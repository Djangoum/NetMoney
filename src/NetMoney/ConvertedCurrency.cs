namespace NetMoney
{
    using Core;
    using System;
    using System.Threading.Tasks;

    internal class ConvertedCurrency : IConvertedCurrency, IEquatable<IConvertedCurrency>
    {
        public decimal Amount { get; private set; }

        public Currency Currency { get; private set; }

        internal IMoney money { get; set; }

        internal DateTime? InternalDate { get; set; }

        public DateTime? Date => InternalDate;

        internal ConvertedCurrency(Currency currency, decimal amount, DateTime? date, IMoney money)
        {
            InternalDate = date;
            Currency = currency;
            Amount = amount;
            this.money = money;
        }

        public async Task<IConvertedCurrency> To(Currency currency)
            => Create(currency, (await money.GetExchangeRatesAsync(Currency, Date, currency)).Rates[currency] * Amount, money);

        public Task<IConvertedCurrency> Sum(IConvertedCurrency currency)
            => Task.FromResult(this + currency);

        public Task<IConvertedCurrency> Sum(Currency currency, decimal amount)
            => Task.FromResult(this + Create(currency, amount, money));

        public Task<IConvertedCurrency> Substract(IConvertedCurrency currency)
            => Task.FromResult(this - currency);

        public Task<IConvertedCurrency> Substract(Currency currency, decimal amount)
                => Task.FromResult(this - Create(currency, amount, money));

        internal static IConvertedCurrency Create(Currency currency, decimal amount, IMoney money)
            => new ConvertedCurrency(currency, amount, null, money);

        public bool Equals(IConvertedCurrency other)
        {
            if (Currency == other.Currency)
            {
                return Amount == other.Amount;
            }
            IConvertedCurrency currency;

            currency = !Date.HasValue ? money.From(other.Currency, other.Amount).To(this.Currency).GetAwaiter().GetResult():
                money.FromDate(other.Currency, other.Amount, Date.Value).To(this.Currency).GetAwaiter().GetResult();

            return Equals(currency);
        }

        public bool Equals(Currency currency, decimal amount)
            => Equals(Create(currency, amount, money));   

        public IConvertedCurrency FromDate(DateTime date)
        {
            InternalDate = date;
            return this;
        }

        public static IConvertedCurrency operator +(ConvertedCurrency currency, IConvertedCurrency addingCurrency)
        {
            if (currency.Currency.Equals(addingCurrency.Currency))
            {
                currency.Amount += addingCurrency.Amount;
                return currency;
            }

            ConvertedCurrency convertedCurrency = (ConvertedCurrency)currency.money.From(addingCurrency.Currency, addingCurrency.Amount).To(currency.Currency).GetAwaiter().GetResult();
            return currency + convertedCurrency;
        }

        public static IConvertedCurrency operator -(ConvertedCurrency currency, IConvertedCurrency substractingCurrency)
        {
            if (currency.Currency.Equals(substractingCurrency.Currency))
            {
                currency.Amount -= substractingCurrency.Amount;
                return currency;
            }

            ConvertedCurrency convertedCurrency = (ConvertedCurrency)currency.money.From(substractingCurrency.Currency, substractingCurrency.Amount).To(currency.Currency).GetAwaiter().GetResult();
            return currency - convertedCurrency;
        }
    }
}
