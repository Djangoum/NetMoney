using NetMoney.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetMoney.MoneyModels
{
    public class ExchangeRates
    {
        public Currency Base { get; set; }
        public DateTime Date { get; set; }
        public IDictionary<Currency, decimal> Rates { get; set; }
    }
}
