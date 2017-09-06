

namespace NetMoney.MoneyModels
{
    using NetMoney.Core;
    using System;

    internal class ExchangeCurrencies
    {
        internal Currency[] To { get; set; }
        internal Currency? From { get; set; }
        internal DateTime? Date { get; set; }

        internal ExchangeCurrencies(Currency? From, Currency[] To, DateTime? Date)
        {
            if (From == null)
                this.From = Currency.EUR;
            else
                this.From = From.Value;

            this.To = To;

            this.Date = Date;
        }
    }
}
