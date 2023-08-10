using FluentAssertions;
using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.Dtos;
using Greggs.Products.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Greggs.Products.UnitTests;

public class ProductsControllerTests
{
    private Mock<ILogger<ProductController>> _loggerMock;
    private Mock<IProductService> _productServiceMock;


    public ProductsControllerTests()
    {
        _loggerMock = new Mock<ILogger<ProductController>>();
        _productServiceMock = new Mock<IProductService>();
    }


    [Fact]
    public void ProductsController_Should_Log_Error_When_Exception_Is_Thrown()
    {
        //Arrange
        _productServiceMock.Setup(x => x.GetProducts(0, 5, "fr")).Throws(new ArgumentNullException("Something happened"));

        //Act
        var sut = new ProductController(_loggerMock.Object, _productServiceMock.Object);

        sut.GetProductsMultiTennent("fr");

        //Assert
        _loggerMock.Verify(x => x.Log(
          LogLevel.Error,
          It.IsAny<EventId>(),
          It.IsAny<It.IsAnyType>(),
          It.IsAny<ArgumentNullException>(),
          It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
          Times.Once);
    }


    [Fact]
    public void ProductsController_Should_Call_Service_Once_with_specified_Paramaters()
    {
        //Arrange
        _productServiceMock.Setup(x => x.GetProducts(0, 5, "fr")).Returns(new List<ProductDto>());

        //Act
        var sut = new ProductController(_loggerMock.Object, _productServiceMock.Object);

        sut.GetProductsMultiTennent("fr");

        //Assert
        _productServiceMock.Verify(X => X.GetProducts(0,5,"fr"), Times.Once());

    }

    [Fact]
    public void ProductsController_Should_Return_An_EmptyList_When_Exception_Is_Thrown()
    {
        //Arrange
        _productServiceMock.Setup(x => x.GetProducts(0, 5, "fr")).Returns(new List<ProductDto>());

        //Act
        var sut = new ProductController(_loggerMock.Object, _productServiceMock.Object);

        var actual = sut.GetProductsMultiTennent("fr");

        //Assert
        actual.Should().BeEquivalentTo(new List<ProductDto>());
    }
}