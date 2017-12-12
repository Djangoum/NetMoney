namespace NetMoney
{
    using Core;
    using MoneyModels;
    using System;
    using System.Threading.Tasks;

    public interface IMoney
    {
        Task<ExchangeRates> GetExchangeRatesAsync(Currency? From = null, DateTime? Date = null, params Currency[] to);
        Task<ExchangeRates> GetExchangeRatesAsync(Currency? From = null, params Currency[] to);
    }
}