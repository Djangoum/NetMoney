namespace NetMoney
{
    using Core;
    using MoneyModels;
    using System;
    using System.Threading.Tasks;

    public interface IMoney
    {
        IConvertedCurrency From(Currency currency, decimal amount);
        IConvertedCurrency FromDate(Currency currency, decimal amount, DateTime date);
        Task<ExchangeRates> GetAllExchangeRatesForEuroTodayAsync();
        Task<ExchangeRates> GetAllExchangeRatesForEuroAsync(DateTime date);
        Task<ExchangeRates> GetExchangeRatesForEuroAsync(DateTime date, params Currency[] to);
        Task<ExchangeRates> GetExchangeRatesAsync(Currency? From, DateTime? Date, params Currency[] to);
        Task<ExchangeRates> GetExchangeRatesAsync(Currency? From, params Currency[] to);
    }
}