using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Services.Product;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests.Services.Product
{

    public static class MockMemoryCacheService
    {
        public static IMemoryCache GetMemoryCache(object expectedValue)
        {
            var mockMemoryCache = new Mock<IMemoryCache>();
            mockMemoryCache
                .Setup(x => x.TryGetValue(Api.Models.Constants.CURRENCY_CONVERSION_CONFIG, out expectedValue))
                .Returns(true);
            return mockMemoryCache.Object;
        }
    }
    public class ProductServicesTest
    {
        private readonly Mock<IMemoryCache> _cacheMock;
        private readonly Mock<IDataAccess<Api.Models.Product>> _dataAccessMock;
        private readonly Mock<ILogger<ProductService>> _loggerMock;
        public ProductServicesTest()
        {
            _cacheMock = new Mock<IMemoryCache>();
            _dataAccessMock = new Mock<IDataAccess<Api.Models.Product>>();
            _loggerMock = new Mock<ILogger<ProductService>>();
        }



        [Fact]
        public void TestBasicParameterValidationForCurrencyExists()
        {
            //arrange
            var service = new ProductService(_dataAccessMock.Object, _cacheMock.Object, _loggerMock.Object);

            //act
            var result = service.CurrencyExists(null);
            //assert
            Assert.False(result);

            //act
            result = service.CurrencyExists("   ");
            //assert
            Assert.False(result);

            //act
            result = service.CurrencyExists("GBP");

            //assert
            Assert.False(true);
        }


        [Fact]
        public void TestCacheNullExists()
        {
            //arrange
            var cache = MockMemoryCacheService.GetMemoryCache(null);
            var service = new ProductService(_dataAccessMock.Object, cache, _loggerMock.Object);

            //act
            var result = service.CurrencyExists("EUR");
            //assert
            Assert.False(result);
        }

        [Fact]
        public void TestCurrencyExists()
        {

            var config = new Api.Models.CurrencyConversionConfig
            {
                BaseCurrency = "GBP",
                CurrencyConversionList = new List<Api.Models.CurrencyConversion>()
                {
                    new Api.Models.CurrencyConversion{ Currency = "EUR" , ConversionRate = (decimal)1.1},
                }
            };
            //arrange
            var cache = MockMemoryCacheService.GetMemoryCache(config);
            var service = new ProductService(_dataAccessMock.Object, cache, _loggerMock.Object);

            //act
            var result = service.CurrencyExists("EUR");
            //assert
            Assert.True(result);


            //act
            result = service.CurrencyExists("YAS");
            //assert
            Assert.False(result);
        }



        [Fact]
        public void GetProductsTest()
        {

            Api.Models.CurrencyConversionConfig config = new Api.Models.CurrencyConversionConfig
            {
                BaseCurrency = "GBP",
                CurrencyConversionList = new List<Api.Models.CurrencyConversion>()
                {
                    new Api.Models.CurrencyConversion{ Currency = "EUR" , ConversionRate = (decimal)1.1},
                }
            };



            //arrange
            _dataAccessMock.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Api.Models.Product>()
            {
                new Api.Models.Product{ Name = "a", Price = (decimal) 1},
                new Api.Models.Product{ Name = "b", Price = (decimal) 2},

            });
            var cache = MockMemoryCacheService.GetMemoryCache(config);
            var service = new ProductService(_dataAccessMock.Object, cache, _loggerMock.Object);

            //act
            var result = service.GetProducts(1, 3, "YAS");
            //assert
            Assert.Equal("GBP", result.Currency);

            //act
            result = service.GetProducts(1, 3, "EUR");
            //assert
            Assert.Equal("EUR", result.Currency);
            Assert.Equal((decimal)1.1, result.Products.ToList()[0].Price);
            Assert.Equal((decimal)2.2, result.Products.ToList()[1].Price);

        }

        [Fact]
        public void GetProductsTestNull()
        {
            Api.Models.CurrencyConversionConfig config = new Api.Models.CurrencyConversionConfig
            {
                BaseCurrency = "GBP",
                CurrencyConversionList = new List<Api.Models.CurrencyConversion>()
                {
                    new Api.Models.CurrencyConversion{ Currency = "EUR" , ConversionRate = (decimal)1.1},
                }
            };



            //arrange
            _dataAccessMock.Setup(x => x.List(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Api.Models.Product>());
            var cache = MockMemoryCacheService.GetMemoryCache(config);
            var service = new ProductService(_dataAccessMock.Object, cache, _loggerMock.Object);

            //act
            var result = service.GetProducts(1, 3, "GBP");
            //assert
            Assert.Null(result);
        }



    }
}

