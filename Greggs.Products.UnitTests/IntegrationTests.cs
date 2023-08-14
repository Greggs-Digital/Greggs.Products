using System.Net.Http;
using System.Threading.Tasks;
using Greggs.Products.Api;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Greggs.Products.UnitTests;

public class IntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;

    public IntegrationTests(WebApplicationFactory<Startup> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_Endpoint()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/Product");

        // Assert
        Assert.Equal(200, ((int)response.StatusCode));
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

        // Act
         response = await client.GetAsync("/Product?pageStart=-1");

        // Assert
        Assert.Equal(400, ((int)response.StatusCode));

        // Act
        response = await client.GetAsync("/Product?pageSize=-1");

        // Assert
        Assert.Equal(400, ((int)response.StatusCode));

        // Act
        response = await client.GetAsync("/Product?currency=sdf");

        // Assert
        Assert.Equal(400, ((int)response.StatusCode));


        // Act
        response = await client.GetAsync("/Product?currency=eur");

        // Assert
        Assert.Equal(200, ((int)response.StatusCode));

        // Act
        response = await client.GetAsync("/Product?currency=EUR");

        // Assert
        Assert.Equal(200, ((int)response.StatusCode));
    }
}