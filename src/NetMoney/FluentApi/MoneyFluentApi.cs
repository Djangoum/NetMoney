using NetMoney.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetMoney
{
    public static class MoneyFluentApi
    {
        public static MoneyFrom From(this Money money, Currency currency, decimal amount)
        {
            return new MoneyFrom(currency, amount, money);
        }
    }
}
