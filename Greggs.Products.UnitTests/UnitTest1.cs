using Xunit;
using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Greggs.Products.UnitTests;

public class UnitTest1
{

    ProductController _controller;
    IDataAccess<Product> _service;
    ILogger _logger;

    public UnitTest1()
    {
        _service = new ProductAccess<Product>();
        _controller = new ProductController((ILogger<ProductController>)_logger, _service);
    }


    [Fact]
    public void GetProducts()
    {
       var result = _controller.Get();
       var resultType = result as OkObjectResult;

       Assert.IsType<OkObjectResult>(result);
       Assert.IsType<List<Product>>(resultType.Value);        
    }

    [Fact]
    public void NoProducts()
    {
       var result = _controller.GetCurrentProducts();       
       var resultType = result as OkObjectResult;

       Assert.IsType<OkObjectResult>(result);
       Assert.IsType<List<Product>>(resultType.Value);    
    }
}