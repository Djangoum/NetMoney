namespace NetMoney
{
    using NetMoney.Core;
    using System;
    using System.Threading.Tasks;

    internal class ConvertedCurrency : IConvertedCurrency, IEquatable<IConvertedCurrency>
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

        public IConvertedCurrency Sum(IConvertedCurrency currency)
        {
            return this + (ConvertedCurrency)currency;
        }

        public IConvertedCurrency Substract(IConvertedCurrency currency)
        {
            return this - (ConvertedCurrency)currency;
        }

        internal static IConvertedCurrency Create(Currency currency, decimal amount, Money money)
        {
            return new ConvertedCurrency(currency, amount, money);
        }

        public bool Equals(IConvertedCurrency other)
        {
            if(Currency == other.Currency)
            {
                return Amount == other.Amount;
            }

            IConvertedCurrency currency = this.money.From(other.Currency, other.Amount).To(this.Currency).GetAwaiter().GetResult();
            return Equals(currency);
        }

        public static IConvertedCurrency operator +(ConvertedCurrency currency, ConvertedCurrency addingCurrency)
        {
            if(currency.Currency == addingCurrency.Currency)
            {
                currency.Amount += addingCurrency.Amount;
                return currency;
            }

            ConvertedCurrency convertedCurrency = (ConvertedCurrency)currency.money.From(addingCurrency.Currency, addingCurrency.Amount).To(currency.Currency).GetAwaiter().GetResult();
            return currency + convertedCurrency;
        }

        public static IConvertedCurrency operator -(ConvertedCurrency currency, ConvertedCurrency addingCurrency)
        {
            if (currency.Currency == addingCurrency.Currency)
            {
                currency.Amount -= addingCurrency.Amount;
                return currency;
            }

            ConvertedCurrency convertedCurrency = (ConvertedCurrency)currency.money.From(addingCurrency.Currency, addingCurrency.Amount).To(currency.Currency).GetAwaiter().GetResult();
            return currency - convertedCurrency;
        }
    }
}
