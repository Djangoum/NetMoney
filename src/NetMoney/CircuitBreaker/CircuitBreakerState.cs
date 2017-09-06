namespace NetMoney.CircuitBreaker
{
    internal enum CircuitBreakerState
    {
        Open,
        Closed,
        HalfOpen
    }
}
