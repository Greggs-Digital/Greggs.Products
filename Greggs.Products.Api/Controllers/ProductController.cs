using System;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductService _productService;


    public ProductController(ILogger<ProductController> logger, IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public ActionResult<GetProducts> Get(int pageStart = 0, int pageSize = 5, string currency = "GBP")
    {
        try
        {
            _logger.Log(LogLevel.Debug, $"Called GET /Product/Get for params pageStart :{pageStart}, pageSize : {pageSize}, currencyCode :{currency} ");
            if (pageStart < 0)
                return BadRequest(Models.Constants.NOT_VALID_PAGE_START);
            if (pageSize <= 0)
                return BadRequest(Models.Constants.NOT_VALID_PAGE_SIZE);

            if (!_productService.CurrencyExists(currency))
                return BadRequest(Models.Constants.NOT_VALID_CURRENCY);


            var data = _productService.GetProducts(pageStart, pageSize, currency);
            if (data == null)
            {
                return NoContent();
            }
            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error occured for GET /Product/Get for params pageStart :{pageStart}, pageSize : {pageSize}, currencyCode :{currency} ");
            throw;
        }
    }
}