using System;
using System.Linq;
using System.Collections.Generic;
using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Respositories;
using Greggs.Products.Api.Services;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests.Controllers
{
    public class ProductController_Tests
    {
        [Fact]
        public void Call_Get_ValidParamsNoCurrencyId_ReturnsExpectedProductDTOs()
        {
            //arrange
            var pageStart = 1;
            var pageLength = 2;
            string currencyId = null;

            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockProductRespository = new Mock<IDataRepository<ProductDTO>>();
            var mockExchangeRateService = new Mock<IExchangeRateService>();

            mockProductRespository.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>()))
              .Returns<int, int>((pStart, pEnd) => CreateRepositoryResult(pStart, pEnd));

            var controllerToTest = new ProductController(mockLogger.Object, mockProductRespository.Object, mockExchangeRateService.Object);

            //act
            var result = controllerToTest.Get(pageStart, pageLength, currencyId);

            //assert
            result.Count().Should().Be(pageLength);
            result.Should().BeEquivalentTo(CreateRepositoryResult(pageStart, pageLength));
        }

        [Fact]
        public void Call_Get_ValidParamsWithCurrencyId_ReturnsExpectedProductDTOWithPriceInEuros()
        {
            //arrange
            var pageStart = 1;
            var pageLength = 2;
            string currencyId = "eur";
            decimal exchangeRate = 1.11m;

            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockProductRespository = new Mock<IDataRepository<ProductDTO>>();
            var mockExchangeRateService = new Mock<IExchangeRateService>();

            mockProductRespository.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>()))
              .Returns<int, int>((pStart, pEnd) => CreateRepositoryResult(pStart, pEnd));

            mockExchangeRateService.Setup(x => x.CalculatePriceInCurrency(It.IsAny<decimal>(), It.IsAny<string>()))
                .Returns<decimal, string>((priceInPounds, currencyId) => currencyId == "eur" ? priceInPounds * exchangeRate : throw new ArgumentException());  // return 0 if expected currencyId not passed through correctly

            var controllerToTest = new ProductController(mockLogger.Object, mockProductRespository.Object, mockExchangeRateService.Object);

            //act
            var result = controllerToTest.Get(pageStart, pageLength, currencyId);

            //assert
            var expectedResult = CreateRepositoryResult(pageStart, pageLength).Select(p => new ProductDTO { Name = p.Name, Price = p.Price * exchangeRate });
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Call_Get_PageStartNegative_ReturnsExpectedProductDTOs()
        {
            //arrange
            var pageStart = -1;
            var pageLength = 2;
            string currencyId = null;

            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockProductRespository = new Mock<IDataRepository<ProductDTO>>();
            var mockExchangeRateService = new Mock<IExchangeRateService>();

            mockProductRespository.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>()))
              .Returns<int, int>((pStart, pEnd) => CreateRepositoryResult(pStart, pEnd));

            var controllerToTest = new ProductController(mockLogger.Object, mockProductRespository.Object, mockExchangeRateService.Object);

            //act
            var result = controllerToTest.Get(pageStart, pageLength, currencyId);

            //assert
            result.Should().BeEquivalentTo(CreateRepositoryResult(0, pageLength));
        }

        [Fact]
        public void Call_Get_PageLengthLessThanOne_ReturnsSingleProductDTOs()
        {
            //arrange
            var pageStart = 2;
            var pageLength = -1;
            string currencyId = null;

            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockProductRespository = new Mock<IDataRepository<ProductDTO>>();
            var mockExchangeRateService = new Mock<IExchangeRateService>();

            mockProductRespository.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>()))
              .Returns<int, int>((pStart, pEnd) => CreateRepositoryResult(pStart, pEnd));

            var controllerToTest = new ProductController(mockLogger.Object, mockProductRespository.Object, mockExchangeRateService.Object);

            //act
            var result = controllerToTest.Get(pageStart, pageLength, currencyId);

            //assert
            var expectedResultsLength = 1;
            result.Count().Should().Be(expectedResultsLength);
            result.Should().BeEquivalentTo(CreateRepositoryResult(pageStart, expectedResultsLength));
        }

        [Fact]
        public void Call_Get_CurrencyIDInvalid_ReturnsEmptyResult()
        {
            //arrange
            var pageStart = 1;
            var pageLength = 2;
            string currencyId = "eur";
            string invalidCurrencyId = "foo";
            decimal exchangeRate = 1.11m;

            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockProductRespository = new Mock<IDataRepository<ProductDTO>>();
            var mockExchangeRateService = new Mock<IExchangeRateService>();

            mockProductRespository.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>()))
              .Returns<int, int>((pStart, pEnd) => CreateRepositoryResult(pStart, pEnd));

            mockExchangeRateService.Setup(x => x.CalculatePriceInCurrency(It.IsAny<decimal>(), currencyId))
                .Returns<decimal, string>((priceInPounds, currencyId) => priceInPounds * exchangeRate);  

            mockExchangeRateService.Setup(x => x.CalculatePriceInCurrency(It.IsAny<decimal>(), invalidCurrencyId))
                .Throws(new ArgumentException());

            var controllerToTest = new ProductController(mockLogger.Object, mockProductRespository.Object, mockExchangeRateService.Object);

            //act
            var result = controllerToTest.Get(pageStart, pageLength, invalidCurrencyId);

            //assert
            result.Count().Should().Be(0);
        }

        private IEnumerable<ProductDTO> CreateRepositoryResult(int startIndex, int numberOfItems)
        {
            return Enumerable.Range(startIndex, numberOfItems).Select(p => new ProductDTO { Name = p.ToString(), Price = 1.00m });
        }
    }
}