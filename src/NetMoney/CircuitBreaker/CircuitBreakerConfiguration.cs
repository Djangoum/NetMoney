namespace NetMoney.CircuitBreaker
{
    using System;

    internal class CircuitBreakerConfiguration
    {
        internal CircuitBreakerConfiguration(TimeSpan? serviceTimeOut, TimeSpan? openTimeOut)
        {
            if (serviceTimeOut == null)
                serviceTimeOut = new TimeSpan(0,0,0,30);

            if (openTimeOut == null)
                openTimeOut = new TimeSpan(0, 0, 0, 10);

            this.ServiceTimeOut = serviceTimeOut.Value;
            this.OpenTimeOut = openTimeOut.Value;
        }

        internal TimeSpan ServiceTimeOut { get; set; }
        internal TimeSpan OpenTimeOut { get; set; }
    }
}
