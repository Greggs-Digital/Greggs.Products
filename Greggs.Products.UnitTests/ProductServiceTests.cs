namespace Greggs.Products.UnitTests;

public class ProductServiceTests
{
    #region User Story 1

    [Fact(Skip = "This was the red i.e. failing test because the method under test had not been implemented")]
    public void GetLatestProducts_NoImplementation_ThrowsNotImplementedException()
    {
        // Arrange
        var mockRepo = new Mock<IDataAccess<Product>>();

        var sut = new ProductService(mockRepo.Object);

        // Act
        var result = () => sut.GetLatestProducts(It.IsAny<int>(), It.IsAny<int>());

        // Assert
        Assert.Throws<NotImplementedException>(result);
    }

    [Fact(Skip = "This was the amber i.e. minimally passing test because the method under test had a basic implementation")]
    public void GetLatestProducts_BasicImplementation_ReturnsSomeProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new() {Name = "Sausage Roll", PriceInPounds = 1m},
            new() {Name = "Vegan Sausage Roll", PriceInPounds = 1.1m}
        };

        var mockProductAccess = new Mock<IDataAccess<Product>>();
        mockProductAccess.Setup(p => p.List(It.IsAny<int>(), It.IsAny<int>())).Returns(products);

        var sut = new ProductService(mockProductAccess.Object);

        // Act
        var result = sut.GetLatestProducts(It.IsAny<int>(), It.IsAny<int>());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(products.Count, result.Count());
    }

    [Fact]
    public void GetLatestProducts_CorrectlyOrderedProducts_ReturnsProductsOrderedByDateDescending()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Name = "Sausage Roll", PriceInPounds = 1m, CreatedDate = DateTime.UtcNow.AddHours(-2)},
            new() { Name = "Bacon Sandwich", PriceInPounds = 1.95m, CreatedDate = DateTime.UtcNow.AddHours(-25) },
            new() { Name = "Vegan Sausage Roll", PriceInPounds = 1.1m, CreatedDate = DateTime.UtcNow.AddHours(-26) },
            new() { Name = "Coca Cola", PriceInPounds = 1.2m, CreatedDate = DateTime.UtcNow.AddDays(-4) },
            new() { Name = "Steak Bake", PriceInPounds = 1.2m, CreatedDate = DateTime.UtcNow.AddDays(-7) },
            new() { Name = "Yum Yum", PriceInPounds = 0.7m, CreatedDate = DateTime.UtcNow.AddDays(-8) },
            new() { Name = "Mexican Baguette", PriceInPounds = 2.1m, CreatedDate = DateTime.UtcNow.AddDays(-27) },
            new() { Name = "Pink Jammie", PriceInPounds = 0.5m, CreatedDate = DateTime.UtcNow.AddMonths(-1) }
        };

        var mockProductAccess = new Mock<IDataAccess<Product>>();
        mockProductAccess.Setup(p => p.ListOrderByDescending(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(products.OrderByDescending(p => p.CreatedDate));

        var sut = new ProductService(mockProductAccess.Object);

        // Act
        var result = sut.GetLatestProducts(It.IsAny<int>(), It.IsAny<int>()).ToList();

        // Assert
        Assert.Equal(result, products);
    }

    [Fact]
    public void GetLatestProducts_IncorrectlyOrderedProducts_ReturnsProductsOrderedByDateDescending()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Name = "Sausage Roll", PriceInPounds = 1m, CreatedDate = DateTime.UtcNow.AddHours(-2)},
            new() { Name = "Vegan Sausage Roll", PriceInPounds = 1.1m, CreatedDate = DateTime.UtcNow.AddHours(-26) },
            new() { Name = "Bacon Sandwich", PriceInPounds = 1.95m, CreatedDate = DateTime.UtcNow.AddHours(-25) },
            new() { Name = "Coca Cola", PriceInPounds = 1.2m, CreatedDate = DateTime.UtcNow.AddDays(-4) },
            new() { Name = "Steak Bake", PriceInPounds = 1.2m, CreatedDate = DateTime.UtcNow.AddDays(-7) },
            new() { Name = "Yum Yum", PriceInPounds = 0.7m, CreatedDate = DateTime.UtcNow.AddDays(-8) },
            new() { Name = "Mexican Baguette", PriceInPounds = 2.1m, CreatedDate = DateTime.UtcNow.AddDays(-27) },
            new() { Name = "Pink Jammie", PriceInPounds = 0.5m, CreatedDate = DateTime.UtcNow.AddMonths(-1) }
        };

        var mockProductAccess = new Mock<IDataAccess<Product>>();
        mockProductAccess.Setup(p => p.ListOrderByDescending(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(products.OrderByDescending(p => p.CreatedDate));

        var sut = new ProductService(mockProductAccess.Object);

        // Act
        var result = sut.GetLatestProducts(It.IsAny<int>(), It.IsAny<int>()).ToList();

        // Assert
        Assert.NotEqual(result, products);
    }

    #endregion


}