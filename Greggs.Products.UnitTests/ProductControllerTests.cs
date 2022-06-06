using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;

namespace Greggs.Products.UnitTests
{
    public class ProductControllerTests
    {
        public ProductControllerTests()
        {

        }

        [Fact]
        public void Successfully_Returns_All_Products()
        {
            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockDataAccess = GetMockDataAccess();
            var priceConverter = new PriceConverter(new ExchangeRateAccess());

            var productController = new ProductController(mockLogger.Object, mockDataAccess.Object, priceConverter);

            var result = productController.Get();            
            result.Result.ShouldBeOfType<OkObjectResult>();

            var products = result.Result as OkObjectResult;
            products.Value.ShouldBeOfType<List<Product>>();

            var productList = products.Value as List<Product>;
            productList.Count.ShouldBe(2);

        }

        [Fact]
        public void Invalid_PageStart_Should_Give_400_Bad_Request()
        {
            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockDataAccess = GetMockDataAccess();
            var priceConverter = new PriceConverter(new ExchangeRateAccess());

            var productController = new ProductController(mockLogger.Object, mockDataAccess.Object, priceConverter);

            var result = productController.Get(-1, 5);

            result.Result.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Invalid_PageSize_Should_Give_400_Bad_Request()
        {
            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockDataAccess = GetMockDataAccess();
            var priceConverter = new PriceConverter(new ExchangeRateAccess());

            var productController = new ProductController(mockLogger.Object, mockDataAccess.Object, priceConverter);

            var result = productController.Get(0, 0);

            result.Result.ShouldBeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Successfully_Return_Converted_Currencies()
        {
            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockDataAccess = GetMockDataAccess();
            var exchangeRateAccess = new ExchangeRateAccess();
            var priceConverter = new PriceConverter(exchangeRateAccess);

            decimal rate = exchangeRateAccess.Get("GBP", "EUR");


            var productController = new ProductController(mockLogger.Object, mockDataAccess.Object, priceConverter);

            var result = productController.Get(0, 2, "EUR");
            result.Result.ShouldBeOfType<OkObjectResult>();

            var products = result.Result as OkObjectResult;
            products.Value.ShouldBeOfType<List<Product>>();

            var productList = products.Value as List<Product>;
            productList.Count.ShouldBe(2);

            productList[0].PriceInRequestedCurrency.IsoCurrencyCode.ShouldBe("EUR");
            productList[0].PriceInRequestedCurrency.Price.ShouldBe(Math.Round(productList[0].PriceInPounds * rate, 2));
        }

        [Fact]
        public void Invalid_Currency_Request_Should_Give_400_Bad_Request()
        {
            var mockLogger = new Mock<ILogger<ProductController>>();
            var mockDataAccess = GetMockDataAccess();
            var priceConverter = new PriceConverter(new ExchangeRateAccess());

            var productController = new ProductController(mockLogger.Object, mockDataAccess.Object, priceConverter);

            var result = productController.Get(0, 10, "EURS");
            result.Result.ShouldBeOfType<BadRequestObjectResult>();
        }

        private Mock<IDataAccess<Product>> GetMockDataAccess()
        {
            var mockDataAccess = new Mock<IDataAccess<Product>>();

            mockDataAccess.Setup(m => m.List(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(
                    new List<Product>()
                    {
                        new Product() { Name = "Product1", PriceInPounds = 1.11m },
                        new Product() { Name = "Product2", PriceInPounds = 2.22m },
                    });

            return mockDataAccess;
        }




    }
}
