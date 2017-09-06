namespace NetMoney
{
    using NetMoney.Core;
    using System.Threading.Tasks;

    public interface IConvertedCurrency
    {
        decimal Amount { get; }
        Currency Currency { get; }
        Task<IConvertedCurrency> To(Currency currency);
        IConvertedCurrency Sum(IConvertedCurrency currency);
        IConvertedCurrency Substract(IConvertedCurrency currency);
    }
}
