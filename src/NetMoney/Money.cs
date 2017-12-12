using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo(assemblyName: "NetMoney.Test, PublicKey=0024000004800000940000000602000000240000525341310004000001000100818b297ecd2fd20160ad91e70b20d40038146f32a1d050fe6f4b2c16ef06570f56c7315adf238a7c64a88d2df65cfe3f70df10f9c10c34bc762368d00d4624af6d71bf625c7ba7c22157ed0307b57a23548710c290153c86eae37f838aafed15a48ffd900216d317ee45783b111a5b5f4bd99557c956bf309a31ae07a6348ac1")]

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