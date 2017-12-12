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
        public async Task Circuit_Throws_CircuitBreakerOpenException_When_Opened_By_TimeOut()
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

        [TestMethod]
        public async Task Circuit_Opended_After_Exception()
        {
            CircuitBreaker<string, string> circuitBreaker = new CircuitBreaker<string, string>(req => throw new Exception(), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5));

            await Assert.ThrowsExceptionAsync<Exception>(() => circuitBreaker.Call("test"));

            Assert.AreEqual(circuitBreaker.State, CircuitBreakerState.Open);
        }

        [TestMethod]
        public async Task Circuit_Half_Open_After_Timeout()
        {
            CircuitBreaker<string, string> circuitBreaker = new CircuitBreaker<string, string>(req => throw new Exception(), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(2));

            await Assert.ThrowsExceptionAsync<Exception>(() => circuitBreaker.Call("test"));

            await Task.Delay(2500);

            Assert.AreEqual(circuitBreaker.State, CircuitBreakerState.HalfOpen);
        }

        [TestMethod]
        public async Task Circuit_Closed_After_Opend_And_HalfOpen_After_Succed()
        {
            bool @throw = true;

            CircuitBreaker<string, string> circuitBreaker = new CircuitBreaker<string, string>(req =>
            {
                if (@throw)
                {
                    @throw = !@throw;
                    throw new Exception();
                }

                return Task.FromResult("ssucced");
            }, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(2));

            await Assert.ThrowsExceptionAsync<Exception>(() => circuitBreaker.Call("test"));

            await Task.Delay(2500);

            await circuitBreaker.Call("test");

            Assert.AreEqual(circuitBreaker.State, CircuitBreakerState.Closed);
        }

        [TestMethod]
        public async Task Circuit_Opened_On_Timeout()
        {
            CircuitBreaker<string, string> circuitBreaker = new CircuitBreaker<string, string>(async req =>
                {
                    await Task.Delay(2000);
                    return await Task.FromResult("test");
                }, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));

            await Assert.ThrowsExceptionAsync<TimeoutException>(() => circuitBreaker.Call("test"));

            Assert.AreEqual(circuitBreaker.State, CircuitBreakerState.Open);
        }
    }
}