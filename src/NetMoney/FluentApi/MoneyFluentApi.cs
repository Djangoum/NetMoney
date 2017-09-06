namespace NetMoney
{
    using NetMoney.Core;

    public static class MoneyFluentApi
    {
        public static MoneyFrom From(this Money money, Currency currency, decimal amount)
        {
            return new MoneyFrom(currency, amount, money);
        }
    }
}
