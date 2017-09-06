using NetMoney.Core;
using NetMoney.MoneyModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetMoney
{
    public class MoneyFrom
    {
        internal Currency From { get; set; }
        internal decimal Amount { get; set; }
        internal Money Money { get; set; }
        internal DateTime? Date { get; set; }

        public MoneyFrom(Currency currency, decimal amount, Money money)
        {
            this.From = currency;
            this.Amount = amount;
            this.Money = money;
        }

        public MoneyFrom FromDate(DateTime date)
        {
            this.Date = date;
            return this;
        }

        public async Task<decimal> To(Currency currency)
        {
            return (await Money.GetExchangeRatesAsync(From, Date, currency)).Rates[currency] * Amount;
        }
    }
}
