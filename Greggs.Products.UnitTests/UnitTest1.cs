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
        public void ConvertCurrency_ConvertsToEuro()
        {
            var currencyConverterMock = new Mock<ICurrencyConverterService>();
            currencyConverterMock.Setup(c => c.ConvertCurrency(It.IsAny<decimal>(), It.IsAny<string>()))
                                 .Returns<decimal, string>((amount, currencyCode) => amount * 1.11m);

            var controller = new ProductController(
                new Mock<ILogger<ProductController>>().Object,
                new Mock<IDataAccess<Product>>().Object,
                currencyConverterMock.Object);
        
            var result = controller.Get(0, 5, "EUR"); 

            Assert.NotNull(result);
  
        }
    }
}
