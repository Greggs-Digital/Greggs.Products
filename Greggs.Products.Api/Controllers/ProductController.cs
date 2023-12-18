using System;
using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    /*private static readonly string[] Products = new[]
    {
        "Sausage Roll", "Vegan Sausage Roll", "Steak Bake", "Yum Yum", "Pink Jammie"
    };*/

    private readonly ILogger<ProductController> _logger;

    private readonly IDataAccess<Product> _product;

    public ProductController(ILogger<ProductController> logger, IDataAccess<Product> product)
    {
        _logger = logger;
        _product = product;
    }

    

    [HttpGet("products", Name ="GetProducst")]
    public ActionResult Get()
    {        
        var products = _product.GetAll();

        if (products.Count == 0)
            return BadRequest();

        return Ok(products);
    }

    [HttpGet("currencyproducts", Name ="GetCurrencyProducts")]
    public ActionResult GetCurrentProducts()
    {
        var exchangeRate = 1.11m;
        var products = _product.GetAll();

        if (products.Count == 0)
            return BadRequest();

        products.AsEnumerable().Sum(p => p.PriceInEuro =  Math.Round(p.PriceInPounds * Convert.ToDecimal(exchangeRate),2));

        return Ok(products);
    }
}