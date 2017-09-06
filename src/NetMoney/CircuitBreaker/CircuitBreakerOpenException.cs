using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NetMoney.CircuitBreaker
{
    /// <summary>
    /// Exception thrown when circuit breaker is called while in either Open state 
    /// or in HalfOpen state for each call except the first one.
    /// </summary>
    public class CircuitBreakerOpenException : Exception
    {
        public CircuitBreakerOpenException()
        {
        }

        public CircuitBreakerOpenException(string message)
            : base(message)
        {
        }

        protected CircuitBreakerOpenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
