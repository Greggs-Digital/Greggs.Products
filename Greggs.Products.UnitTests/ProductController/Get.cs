using System;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;
using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;

namespace Greggs.Products.UnitTests;

public class ProductController_Get
{
    [Fact]
    public void BadPageStart()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProductController>>();
        var dataAccess = Mock.Of<IDataAccess<Product>>();
        var target = new ProductController(logger, dataAccess);
        Action act = () => { target.Get(-1, default); };

        // Act
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void BadPageSize()
    {
        // Arrange
        var logger = Mock.Of<ILogger<ProductController>>();
        var dataAccess = Mock.Of<IDataAccess<Product>>();
        var target = new ProductController(logger, dataAccess);
        Action act = () => { target.Get(default, -1); };

        // Act
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void NoData()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<ProductController>>();
        var mockDataAccess = new Mock<IDataAccess<Product>>();
        var target = new ProductController(mockLogger.Object, mockDataAccess.Object);

        int pageStart = 10;
        int pageSize = 100;

        // Act
        var result = target.Get(pageStart, pageSize);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(0);
        mockDataAccess.Verify(t => t.List(pageStart, pageSize), Times.Once);
    }

    [Fact]
    public void SomeData()
    {
        // Arrange
        int pageStart = 50;
        int pageSize = 50;
        var mockLogger = new Mock<ILogger<ProductController>>();
        var mockDataAccess = new Mock<IDataAccess<Product>>();
        mockDataAccess
            .Setup(t => t.List(pageStart, pageSize))
            .Returns(
                () => new []
                {
                    new Product { Name = "A", PriceInPounds = 1.0m },
                    new Product { Name = "A", PriceInPounds = 1.0m },
                    new Product { Name = "A", PriceInPounds = 1.0m },
                });
        var target = new ProductController(mockLogger.Object, mockDataAccess.Object);



        // Act
        var result = target.Get(pageStart, pageSize);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        mockDataAccess.Verify(t => t.List(pageStart, pageSize), Times.Once);
    }
}