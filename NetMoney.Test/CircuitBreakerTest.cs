namespace NetMoney.Test
{
    using CircuitBreaker;
    using Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using MoneyModels;
    using Moq;
    using System;
    using System.Threading.Tasks;

    [TestClass]
    public class CircuitBreakerTest
    {
        [TestMethod]
        public async Task Circuit_Throws_On_Timeout_Exception()
        {
            IMoney moneySingleton = default(IMoney).Create(0, 10);
            
            await Assert.ThrowsExceptionAsync<TimeoutException>(() => moneySingleton.GetExchangeRatesAsync(Currency.USD, Currency.EUR));
        }

        [TestMethod]
        public async Task Circuit_Throws_On_Service_Exception()
        {
            var IMoneyMock = new Mock<Money>(MockBehavior.Strict, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10));

            IMoneyMock.Object.circuitBreaker = new NetMoney.CircuitBreaker.CircuitBreaker<ExchangeCurrencies, ExchangeRates>(
                req => throw new Exception(),
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(10));

            await Assert.ThrowsExceptionAsync<Exception>(() => IMoneyMock.Object.GetExchangeRatesAsync(Currency.EUR, Currency.AUD));
        }

        [TestMethod]
        public async Task Circuit_Opens_on_Timeout_Exception()
        {
            IMoney moneySingleton = default(IMoney).Create(0, 10);

            await Assert.ThrowsExceptionAsync<TimeoutException>(() => moneySingleton.GetExchangeRatesAsync(Currency.USD, Currency.EUR));

            await Assert.ThrowsExceptionAsync<CircuitBreakerOpenException>(() => moneySingleton.GetExchangeRatesAsync(Currency.USD, Currency.EUR));
        }

        [TestMethod]
        public async Task Circuit_Close_After_Close_TimeOut()
        {
            IMoney moneySingleton = default(IMoney).Create(0, 3);

            await Assert.ThrowsExceptionAsync<TimeoutException>(() => moneySingleton.GetExchangeRatesAsync(Currency.USD, Currency.EUR));

            await Assert.ThrowsExceptionAsync<CircuitBreakerOpenException>(() => moneySingleton.GetExchangeRatesAsync(Currency.USD, Currency.EUR));

            await Task.Delay(3500);

            await Assert.ThrowsExceptionAsync<TimeoutException>(() => moneySingleton.GetExchangeRatesAsync(Currency.USD, Currency.EUR));
        }
    }
}