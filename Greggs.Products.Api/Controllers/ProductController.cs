using System;
using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Exceptions;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IDataAccess<Product> _productDataAccess;
    private readonly IPriceConverter _priceConverter;

    public ProductController(ILogger<ProductController> logger, IDataAccess<Product> productDataAccess, IPriceConverter priceConverter)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _productDataAccess = productDataAccess ?? throw new ArgumentNullException(nameof(productDataAccess));
        _priceConverter = priceConverter ?? throw new ArgumentNullException(nameof(priceConverter));
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> Get(int pageStart = 0, int pageSize = 5, string isoCurrencyCode = "")
    {
        if(pageStart < 0 || pageSize <= 0)
        {
           return BadRequest("Invalid paging data");
        }

        var products = _productDataAccess.List(pageStart, pageSize);

        if(!string.IsNullOrEmpty(isoCurrencyCode))
        {
            try
            {
                products = _priceConverter.UpdateProductsWithRequestedCurrency(products, isoCurrencyCode);
            }
            catch (CurrencyConversionException ccx)
            {
                return BadRequest(ccx.Message);
            }
        }

        return Ok(products);


    }
}