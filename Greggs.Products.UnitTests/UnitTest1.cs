using System.Collections.Generic;
using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void ReturnsProducts()
        {
            var loggerMock = new Mock<ILogger<ProductController>>();
            var dataAccessMock = new Mock<IDataAccess<Product>>();
            var currencyConverterMock = new Mock<ICurrencyConverterService>();

            var controller = new ProductController(loggerMock.Object, dataAccessMock.Object, currencyConverterMock.Object);

            dataAccessMock.Setup(d => d.List(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Product>());

            var result = controller.Get();

            Assert.NotNull(result);
        }

        [Fact]
        public void CurrencyConverter()
        {
            var converterService = new CurrencyConverterService();

            var result = converterService.ConvertToEuro(10);

            Assert.Equal(11.1m, result);
        }
    }
}
