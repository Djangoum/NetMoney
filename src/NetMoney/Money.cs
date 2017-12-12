using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo(assemblyName: "NetMoney.Test")]

namespace NetMoney
{
    using CircuitBreaker;
    using Core;
    using NetMoney.MoneyModels;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class Money : IMoney
    {
        internal CircuitBreaker<ExchangeCurrencies, ExchangeRates> circuitBreaker { get; set; }

        internal Money(CircuitBreakerConfiguration configuration) 
            => circuitBreaker = new CircuitBreaker<ExchangeCurrencies, ExchangeRates>(
                req => GetFixerIoRates(req),
                configuration.ServiceTimeOut,
                configuration.OpenTimeOut);

        public Money(TimeSpan? serviceTimeOut, TimeSpan? openTimeOut) : this(new CircuitBreakerConfiguration(serviceTimeOut, openTimeOut)) { }

        public Money(int serviceTimeOutSeconds, int openTimeOutSeconds) : this(new TimeSpan(0, 0, serviceTimeOutSeconds), new TimeSpan(0, 0, openTimeOutSeconds)) { }

        public async Task<ExchangeRates> GetExchangeRatesAsync(Currency? From = null, DateTime? Date = null, params Currency[] to)
            => await circuitBreaker.Call(new ExchangeCurrencies(From, to, Date));

        public async Task<ExchangeRates> GetExchangeRatesAsync(Currency? From = null, params Currency[] to)
            => await circuitBreaker.Call(new ExchangeCurrencies(From, to, null));

        internal async Task<ExchangeRates> GetFixerIoRates(ExchangeCurrencies exchangeCurrencies)
        {
            string uri = FixerIo.FixerIoEndPoint;

            if (exchangeCurrencies.Date != null)
            {
                uri += $"{exchangeCurrencies.Date.Value.ToString("yyyy-MM-dd")}";
            }
            else
            {
                uri += "latest";
            }

            uri += $"?base={exchangeCurrencies.From}";

            if (exchangeCurrencies.To.Count() != 0)
            {
                uri += "&symbols=";

                foreach (Currency currency in exchangeCurrencies.To)
                {
                    if (Array.IndexOf(exchangeCurrencies.To, currency) != (exchangeCurrencies.To.Count() - 1))
                    {
                        uri += $"{currency.ToString()},";
                        continue;
                    }

                    uri += $"{currency.ToString()}";
                }
            }

            return await HttpClientWrapper.Get<ExchangeRates>(uri);
        }
    }
}