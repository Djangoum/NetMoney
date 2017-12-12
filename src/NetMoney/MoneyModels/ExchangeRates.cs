namespace NetMoney.MoneyModels
{
    using Core;
    using System;
    using System.Collections.Generic;

    public class ExchangeRates
    {
        public Currency Base { get; set; }
        public DateTime Date { get; set; }
        public IDictionary<Currency, decimal> Rates { get; set; }
    }
}
