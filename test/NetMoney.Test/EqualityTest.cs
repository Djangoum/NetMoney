﻿namespace NetMoney.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;

    [TestClass]
    public class EqualityTest
    {
        [TestMethod]
        public void Equals_Same_Currency()
        {
            IMoney money = new Money(10, 5);

            var eurResult = money.From(Core.Currency.EUR, 1003.65m);

            Assert.IsTrue(eurResult.Equals(Core.Currency.EUR, 1003.65m));
        }

        [TestMethod]
        public async Task Equals_Different_Currency()
        {
            IMoney money = new Money(10, 5);

            var eurResult = money.From(Core.Currency.EUR, 1003.64m);

            var sekResult = await eurResult.To(Core.Currency.SEK);

            var gpbResult = await eurResult.To(Core.Currency.GBP);

            sekResult.Equals(gpbResult);
        }

        [TestMethod]
        public void Equals_Returning_False_Same_Currency()
        {
            IMoney money = new Money(10, 5);

            var eurResult = money.From(Core.Currency.EUR, 0.65m);

            Assert.IsFalse(eurResult.Equals(Core.Currency.EUR, 1003.65m));
        }

    }
}
