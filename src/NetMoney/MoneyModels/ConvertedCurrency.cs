namespace NetMoney.MoneyModels
{
    using NetMoney.Core;
    using System.Threading.Tasks;


    internal class ConvertedCurrency : IConvertedCurrency
    {
        public decimal Amount { get; private set; }

        public Currency Currency { get; private set; }

        internal Money money { get; set; }

        internal ConvertedCurrency(Currency currency, decimal amount, Money money)
        {
            Currency = currency;
            Amount = amount;
            this.money = money;
        }

        public async Task<IConvertedCurrency> To(Currency currency)
        {
            return await money.From(Currency, Amount).To(currency);
        }

        internal static IConvertedCurrency Create(Currency currency, decimal amount, Money money)
        {
            return new ConvertedCurrency(currency, amount, money);
        }
    }
}
