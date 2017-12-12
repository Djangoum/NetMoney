namespace NetMoney
{
    using Core;
    using System;
    using System.Threading.Tasks;

    public interface IConvertedCurrency
    {
        decimal Amount { get; }
        Currency Currency { get; }
        DateTime? Date { get; }
        IConvertedCurrency FromDate(DateTime date);
        Task<IConvertedCurrency> To(Currency currency);
        Task<IConvertedCurrency> Sum(IConvertedCurrency currency);
        Task<IConvertedCurrency> Substract(IConvertedCurrency currency);
        Task<IConvertedCurrency> Sum(Currency currency, decimal amount);
        Task<IConvertedCurrency> Substract(Currency currency, decimal amount);
        bool Equals(Currency currency, decimal amount);
        bool Equals(IConvertedCurrency other);
    }
}
