using System;
using System.Collections.Generic;
using System.Text;

namespace NetMoney.Core
{
    public enum Currency
    {
        EUR,
        AUD,
        BGN,
        BRL,
        CAD,
        CHF,
        CNY,
        CZK,
        DKK,
        GBP,
        HKD,
        HRK,
        HUF,
        IDR,
        ILS,
        INR,
        JPY,
        KRW,
        MXN,
        MYR,
        NOK,
        NZD,
        PHP,
        PLN,
        RON,
        RUB,
        SEK,
        SGD,
        THB,
        TRY,
        USD,
        ZAR
    }

    internal static class FixerIo
    {
        internal const string FixerIoEndPoint = "http://api.fixer.io/";
    }
}
