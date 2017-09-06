using System;
using System.Collections.Generic;
using System.Text;

namespace NetMoney.CircuitBreaker
{
    internal enum CircuitBreakerState
    {
        Open,
        Closed,
        HalfOpen
    }
}
