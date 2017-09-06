namespace NetMoney.MoneyModels
{
    using NetMoney.Core;

    public interface IConvertedCurrency
    {
        decimal Amount { get; }
        Currency Currency { get; }
    }
}
