using System;
using FluentAssertions;
using Greggs.Products.Api.Services;
using Xunit;

namespace Greggs.Products.UnitTests.Services
{
    public class ExchangeRateService_Tests
    {
        private readonly decimal _expectedEurRate = 1.11m;
        private readonly string _eurCurrencyId = "eur";

        [Theory]
        [InlineData(0.01)]
        [InlineData(1.00)]
        [InlineData(2.00)]
        public void Call_CalculatePriceInCurrency_ReturnsExpectedValue(decimal priceInPounds)
        {
            // arrange
            var exchangeRateServiceToTest = new ExchangeRateService();

            // act
            var result = exchangeRateServiceToTest.CalculatePriceInCurrency(priceInPounds, _eurCurrencyId);

            // assert
            result.Should().Be(priceInPounds * _expectedEurRate);

        }

        [Theory]
        [InlineData(-0.01)]
        [InlineData(-1.00)]
        public void Call_CalculatePriceInCurrency_NegativePrice_Throws(decimal priceInPounds)
        {
            // arrange
            var exchangeRateServiceToTest = new ExchangeRateService();

            // act and assert
            exchangeRateServiceToTest.Invoking(p => p.CalculatePriceInCurrency(priceInPounds, _eurCurrencyId))
                                     .Should()
                                     .Throw<ArgumentException>()
                                     .WithMessage("CalculatePriceInCurrency priceInPounds < 0.00 found");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Call_CalculatePriceInCurrency_NullAndEmptyCurrency_Throws(string currencyId)
        {
            // arrange
            var priceInPounds = 1.00m;
            var exchangeRateServiceToTest = new ExchangeRateService();

            // act and assert
            exchangeRateServiceToTest.Invoking(p => p.CalculatePriceInCurrency(priceInPounds, currencyId))
                                     .Should()
                                     .Throw<ArgumentNullException>()
                                     .WithMessage("Value cannot be null. (Parameter 'currencyId')");
        }

        [Theory]
        [InlineData("foo")]
        [InlineData("bar")]
        public void Call_CalculatePriceInCurrency_NonExistantCurrencyId_Throws(string currencyId)
        {
            // arrange
            var priceInPounds = 1.00m;
            var exchangeRateServiceToTest = new ExchangeRateService();

            // act and assert
            exchangeRateServiceToTest.Invoking(p => p.CalculatePriceInCurrency(priceInPounds, currencyId))
                                     .Should()
                                     .Throw<ArgumentException>()
                                     .WithMessage($"Currency with id '{currencyId}' not found");
        }
    }
}