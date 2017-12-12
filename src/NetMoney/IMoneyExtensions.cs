namespace NetMoney
{
    using Core;
    using System;

    public static class IMoneyExtensions
    {
        public static IMoney Create(this IMoney money, TimeSpan? serviceTimeOut, TimeSpan? openTimeOut)
        {
            return new Money(serviceTimeOut, openTimeOut);
        }

        public static IMoney Create(this IMoney money, int serviceTimeOutSeconds, int openTimeOutSeconds)
        {
            return new Money(serviceTimeOutSeconds, openTimeOutSeconds);
        }

        public static IConvertedCurrency From(this IMoney money, Currency currency, decimal amount)
             => new ConvertedCurrency(currency, amount, null, money);

        public static IConvertedCurrency FromDate(this IMoney money, Currency currency, decimal amount, DateTime date)
            => new ConvertedCurrency(currency, amount, date, money);
    }
}
