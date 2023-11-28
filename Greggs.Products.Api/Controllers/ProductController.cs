using System;
using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Greggs.Products.Api.DataAccess;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{   
    /**********************************************************************
    private static readonly string[] Products = new[]
    {
        "Sausage Roll", "Vegan Sausage Roll", "Steak Bake", "Yum Yum", "Pink Jammie"
    };
    ***************************************************************************/


    private readonly ILogger<ProductController> _logger;
    private readonly IDataAccess<Product> _productAccess;
    
    public ProductController(ILogger<ProductController> logger, IDataAccess<Product> productAccess)
    {
        _logger = logger;
        _productAccess = productAccess;
    }

    
    [HttpGet]
    public IEnumerable<Product> Get(int pageStart = 0, int pageSize = 5)
    {
        var products = _productAccess.List(pageStart, pageSize);
            
        // Calculate Euro prices using a fixed exchange rate of 1GBP to 1.11EUR
        foreach (var product in products)
        {
            product.PriceInEuros = product.PriceInPounds * 1.11m;
        }

        return products;
    }
    
}