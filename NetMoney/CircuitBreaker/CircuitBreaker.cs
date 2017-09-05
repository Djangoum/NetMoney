using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetMoney.CircuitBreaker
{
    internal class CircuitBreaker<TReq, TRep>
    {
        // _state and _wasProbed are ints because of use Interlocked methods

        private int _state = (int)CircuitBreakerState.Closed;

        /// <summary>
        /// Flag determining if first call was requested while in HalfOpen state.
        /// </summary>
        private int _wasProbed = 0;

        /// <summary>
        /// Service to be handled by the circuit breaker.
        /// </summary>
        private readonly Func<TReq, Task<TRep>> _service;

        /// <summary>
        /// Timeout for service call. If service won't respond until timeout occurs, 
        /// circuit breaker becomes <see cref="CircuitBreakerState.Closed"/>.
        /// </summary>
        internal readonly TimeSpan ServiceTimeout;

        /// <summary>
        /// Timeout for closed state. After switching to <see cref="CircuitBreakerState.Closed"/>,
        /// circuit breaker can remain in that state only for specified timeout. Then it becomes
        /// <see cref="CircuitBreakerState.HalfOpen"/>.
        /// </summary>
        internal readonly TimeSpan OpenedTimeout;

        /// <summary>
        /// Create new instance of the <see cref="CircuitBreaker"/> class working as a facade
        /// to access provided <paramref name="service"/> call.
        /// </summary>
        /// <param name="service"></param>
        /// <param name="serviceTimeout">Time limit given to <paramref name="service"/> to execute.</param>
        /// <param name="openedTimeout">
        /// Defines an interval, after which circuit will automatically switch from Open to HalfOpen state.
        /// </param>
        internal CircuitBreaker(Func<TReq, Task<TRep>> service, TimeSpan serviceTimeout, TimeSpan openedTimeout)
        {
            _service = service;
            ServiceTimeout = serviceTimeout;
            OpenedTimeout = openedTimeout;
        }

        /// <summary>
        /// Gets current state of the circuit breaker.
        /// </summary>
        internal CircuitBreakerState State
        {
            get { return (CircuitBreakerState)_state; }
        }

        /// <summary>
        /// Performs an async call to underlying service. If it will fail or won't finish in specified 
        /// <see cref="ServiceTimeout"/>, either inner exception of <see cref="TimeoutException"/> will
        /// be thrown and circuit will switch to <see cref="CircuitBreakerState.Open"/> state.
        /// 
        /// Circuit breaker may stay in <see cref="CircuitBreakerState.Open"/> state for time specified
        /// in <see cref="OpenedTimeout"/> field. While in that state any following calls will result in
        /// task ending with <see cref="CircuitBreakerOpenException"/> being thrown. After 
        /// <see cref="OpenedTimeout"/> circuit will come into <see cref="CircuitBreakerState.HalfOpen"/> state.
        /// 
        /// While in <see cref="CircuitBreakerState.HalfOpen"/> state first call will result in calling
        /// an underlying service. Any subsequent calls will result in task with 
        /// <see cref="CircuitBreakerOpenException"/> being thrown inside. If service will fail after 
        /// calling, circuit switches back to <see cref="CircuitBreakerState.Open"/> state for specified
        /// timeout. If call succeeds, it switches back to <see cref="CircuitBreakerState.Closed"/> state. 
        /// </summary>
        internal Task<TRep> Call(TReq request)
        {
            switch (State)
            {
                case CircuitBreakerState.Closed: return CallClosed(request);
                case CircuitBreakerState.Open: return CallOpen(request);
                case CircuitBreakerState.HalfOpen: return CallHalfOpen(request);
                default: throw new NotSupportedException("Circuit breaker don't support state of " + State);
            }
        }

        private Task<TRep> CallHalfOpen(TReq request)
        {
            // pass first call to underlying service, and block all others
            if (Interlocked.CompareExchange(ref _wasProbed, 1, 0) == 0)
            {
                return CallClosed(request, true);
            }
            else
            {
                return CallOpen(request);
            }
        }

        private async Task<TRep> CallClosed(TReq request, bool resetState = false)
        {
            var cancellation = new CancellationTokenSource(ServiceTimeout);
            var serviceTask = _service(request);
            var task = await Task.WhenAny(new Task[] { serviceTask, Task.Delay(ServiceTimeout, cancellation.Token) });

            if (task == serviceTask)
            {
                if (task.IsFaulted)
                {
                    // if task failed, open circuit
                    Exception exc;
                    if (serviceTask.Exception != null)
                    {
                        exc = serviceTask.Exception.InnerExceptions.First();
                        BecomeOpen();
                        throw exc;
                    }
                }

                if (resetState) BecomeClosed();

                return serviceTask.Result;
            }
            else
            {
                // when task didn't finish in specified timeout, open circuit
                BecomeOpen();
                throw new TimeoutException(string.Format("Circuit breaker timed out while waiting for underlying service to finish in {0} timeout", ServiceTimeout));
            }
        }

        private async Task<TRep> CallOpen(TReq request)
        {
            throw await Task.FromResult(new CircuitBreakerOpenException("Circuit breaker is open. Default time left for become half opened is " + OpenedTimeout));
        }

        private void BecomeClosed()
        {
            Interlocked.Exchange(ref _state, (int)CircuitBreakerState.Closed);
            _wasProbed = 0;
        }

        private void BecomeHalfOpen(CancellationTokenSource source)
        {
            Interlocked.Exchange(ref _state, (int)CircuitBreakerState.HalfOpen);
            source.Cancel();
        }

        private void BecomeOpen()
        {
            Interlocked.Exchange(ref _state, (int)CircuitBreakerState.Open);

            // setup task for switching to HalfOpen state
            var becomeHalfOpenCancellation = new CancellationTokenSource();
            Task.Delay(OpenedTimeout, becomeHalfOpenCancellation.Token).ContinueWith(t => BecomeHalfOpen(becomeHalfOpenCancellation), becomeHalfOpenCancellation.Token);
        }
    }
}
