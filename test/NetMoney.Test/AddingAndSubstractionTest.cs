using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetMoney.Test
{
    [TestClass]
    public class AddingAndSubstractionTest
    {
        [TestMethod]
        public async Task Add_Same_Currency()
        {
            IMoney money = default(IMoney).Create(10, 5);

            var result = money.From(Core.Currency.EUR, 102.34m);

            await result.Sum(Core.Currency.EUR, 100m);

            Assert.AreEqual(result.Amount, 202.34m);
        }

        [TestMethod]
        public async Task Substract_Same_Currency()
        {
            IMoney money = default(IMoney).Create(10, 5);

            var result = money.From(Core.Currency.EUR, 102.34m);

            await result.Substract(Core.Currency.EUR, 102.34m);

            Assert.AreEqual(result.Amount, 0m);
        }

        [TestMethod]
        public async Task Sum_Different_Currency()
        {
            IMoney money = default(IMoney).Create(10, 5);

            var result = money.From(Core.Currency.EUR, 102.34m);

            await result.Sum(Core.Currency.GBP, 100m);

            Assert.AreNotEqual(result.Amount, 202.34m);
        }

        [TestMethod]
        public async Task Substract_Different_Currency()
        {
            IMoney money = default(IMoney).Create(10, 5);

            var result = money.From(Core.Currency.EUR, 102.34m);

            await result.Substract(Core.Currency.GBP, 100m);

            Assert.AreNotEqual(result.Amount, 2.34m);
        }
    }
}
