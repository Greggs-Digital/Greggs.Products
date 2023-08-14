using System;
using Xunit;
using Moq;
using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.Services.Product;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Greggs.Products.Api.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Greggs.Products.UnitTests.controllers
{
    public class ProductControllerTest
    {
        private Mock<ILogger<ProductController>> _loggerMock;
        private Mock<IProductService> _productServiceMock;
        public ProductControllerTest()
        {
            _loggerMock = new Mock<ILogger<ProductController>>();
            _productServiceMock = new Mock<IProductService>();
        }

        [Fact]
        public void TestInvalidPageStart()
        {
            //arrange
            var controller = new ProductController(_loggerMock.Object, _productServiceMock.Object);

            //act
            var result = controller.Get(-5, 3);

            //assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Invalid page start", errorMessage);
        }

        [Fact]
        public void TestInvalidPageSize()
        {
            //arrange
            var controller = new ProductController(_loggerMock.Object, _productServiceMock.Object);

            //act
            var result = controller.Get(5, -3);

            //assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Invalid page size", errorMessage);
        }


        [Fact]
        public void TestInvalidCurrencyCode()
        {

            //arrange
            _productServiceMock.Setup(x => x.CurrencyExists(It.IsAny<string>())).Returns(false);
            var controller = new ProductController(_loggerMock.Object, _productServiceMock.Object);

            //act
            var result = controller.Get(5, 3, "yas");

            //assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal("Invalid Currency Code", errorMessage);
        }

        [Fact]
        public void TestNoContent()
        {

            //arrange
            _productServiceMock.Setup(x => x.CurrencyExists(It.IsAny<string>())).Returns(true);
            _productServiceMock.Setup(x => x.GetProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns<GetProducts>(null);

            var controller = new ProductController(_loggerMock.Object, _productServiceMock.Object);

            //act
            var result = controller.Get(5, 3, "yas");

            //assert
            Assert.IsType<NoContentResult>(result);

        }

        [Fact]
        public void TestInternalServerError()
        {

            //arrange
            _productServiceMock.Setup(x => x.CurrencyExists(It.IsAny<string>())).Returns(true);
            _productServiceMock.Setup(x => x.GetProducts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(
                new GetProducts
                {
                    PageOffset = 5,
                    PageSize = 3,
                    ProductsFetched = 1,
                    Products = new List<Product>()
                    {
                        new Product{ Name = "a", Price = 1},
                        new Product{Name = "b", Price = 2}
                    }
                });

            var controller = new ProductController(_loggerMock.Object, _productServiceMock.Object);

            //act
            var result = controller.Get(5, 3, "GBP");

            //assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<GetProducts>(okResult.Value);
            Assert.Equal(2, data.Products.Count());

        }


    }
}

