using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace NetMoney.Test
{
    [TestClass]
    public class FluentApiTest
    {
        [TestMethod]
        public void Money_From_Returns_A_Valid_From_Money()
        {
            IMoney money = default(IMoney).Create(10, 5);

            var result = money.From(Core.Currency.EUR, 102.34m);

            Assert.AreEqual(result.Amount, 102.34m);
            Assert.AreEqual(result.Currency, Core.Currency.EUR);
            Assert.AreEqual(result.Date, null);
        }

        [TestMethod]
        public async Task Money_From_To_Returns_A_Valid_Converted_Currency()
        {
            IMoney money = default(IMoney).Create(10, 5);

            var result = await money.From(Core.Currency.EUR, 102.34m).To(Core.Currency.CNY);

            Assert.AreNotEqual(result.Currency, Core.Currency.EUR);
            Assert.AreNotEqual(result.Amount, 102.34m);
        }

        [TestMethod]
        public async Task Money_From_To_From_Returns_Same_Amount()
        {
            IMoney money = default(IMoney).Create(10, 5);

            var result = await money.From(Core.Currency.EUR, 102.32m).To(Core.Currency.CNY);

            var eurResult = await result.To(Core.Currency.EUR);

            Assert.AreEqual(Math.Round(eurResult.Amount, 2), 102.32m);
            Assert.AreEqual(result.Currency, Core.Currency.CNY);
        }

        [TestMethod]
        public async Task Money_From_To_Multiple_Conversions()
        {
            IMoney money = default(IMoney).Create(10, 5);

            var result = await money.From(Core.Currency.EUR, 102.32m).To(Core.Currency.CNY);

            var resultUSD = await result.To(Core.Currency.USD);

            var resultSEK = await resultUSD.To(Core.Currency.SEK);

            var resultGBP = await resultSEK.To(Core.Currency.GBP);

            var resultEUR = await resultGBP.To(Core.Currency.EUR);

            Assert.AreEqual(Math.Round(resultEUR.Amount, 2), 102.32m);
        }

        [TestMethod]
        public async Task Money_From_Date()
        {
            IMoney money = default(IMoney).Create(10, 5);

            var result = money.From(Core.Currency.EUR, 102.34m);

            var resultFromDate = money.FromDate(Core.Currency.EUR, 102.34m, DateTime.Now.AddDays(-72));

            Assert.AreNotEqual(
                (await result.To(Core.Currency.GBP)).Amount,
                (await resultFromDate.To(Core.Currency.GBP)).Amount);
        }
    }
}
