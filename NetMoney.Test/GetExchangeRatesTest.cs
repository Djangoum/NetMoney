namespace NetMoney.Test
{
    using Moq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Core;
    using System.Threading.Tasks;
    using System;

    [TestClass]
    public class GetExchangeRatesTest
    {
        [TestMethod]
        public async Task GetExchangeRatesAsync_With_3_To_Parameters_One_Is_Origin()
        {
            IMoney moneySingleton = default(IMoney).Create(10, 5);
            
            var result = await moneySingleton.GetExchangeRatesAsync(Currency.EUR, Currency.AUD, Currency.CNY, Currency.EUR);

            Assert.AreEqual(result.Rates.Count, 2);
        }

        [TestMethod]
        public async Task GetExchangeRatesAsync_With_no_To_Parameter()
        {
            IMoney moneySingleton = default(IMoney).Create(10, 5);

            var result = await moneySingleton.GetExchangeRatesAsync(Currency.EUR);

            Assert.AreEqual(result.Rates.Count, 31);
        }

        [TestMethod]
        public async Task GetExchangeRatesAsync_With_Date()
        {
            IMoney moneySingleton = default(IMoney).Create(10, 5);

            var result = await moneySingleton.GetExchangeRatesAsync(Currency.EUR, DateTime.Now.AddDays(-74));

            Assert.AreEqual(result.Date.Date, DateTime.Now.AddDays(-74).Date);
        }
    }
}