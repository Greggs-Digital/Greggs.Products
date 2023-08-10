using System;
using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.Dtos;
using Greggs.Products.Api.ExtensionMethods;
using Greggs.Products.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{

    private readonly IProductService _productService;

    private static readonly string[] Products = new[]
    {
        "Sausage Roll", "Vegan Sausage Roll", "Steak Bake", "Yum Yum", "Pink Jammie"
    };

    private readonly ILogger<ProductController> _logger;

    public ProductController(ILogger<ProductController> logger,
                           IProductService productService)
    {
        _logger = logger;
        _productService = productService;
    }

    [HttpGet]
    [Route("/V1/Get")]
    public IEnumerable<ProductDto> Get(int pageStart = 0, int pageSize = 5)
    {
        if (pageSize > Products.Length)
            pageSize = Products.Length;

        var rng = new Random();
        return Enumerable.Range(1, pageSize).Select(index => new ProductDto
            {
                PriceInPounds = rng.Next(0, 10),
                Name = Products[rng.Next(Products.Length)]
            })
            .ToArray();
    }

    [HttpGet]
    [Route("/V2/Get")]
    public IEnumerable<ProductDto> GetProducts(int pageStart = 0, int pageSize = 5)
    {
        try
        {
            _logger.Log(LogLevel.Debug, $"{GetProducts} endpoint has been invoked with following paramaters: {nameof(pageStart)}{pageStart}, {nameof(pageSize)}:{pageSize}");

            pageStart.ValidateNumber();
            pageSize.ValidateNumber();
            _logger.Log(LogLevel.Debug, "Calling product service");

            return _productService.GetProducts(pageStart, pageSize);

        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, $"Exception thrown in {nameof(Get)} type: {e?.GetType()} message: {e?.Message}");

            Response.StatusCode = 400;

            return new List<ProductDto>();
        }
    }

    [HttpGet]
    [Route("/V3/Get")]
    public IEnumerable<ProductDto> GetProductsMultiTennent(string tennent, int pageStart = 0, int pageSize = 5)
    {
        try
        {
            _logger.Log(LogLevel.Debug, $"{GetProductsMultiTennent} endpoint has been invoked with following paramaters:{nameof(tennent)}:{tennent} {nameof(pageStart)}:{pageStart}, {nameof(pageSize)}:{pageSize}");

            tennent.ValidateTenent();
            pageStart.ValidateNumber();
            pageSize.ValidateNumber();

            _logger.Log(LogLevel.Debug, "Calling product service");

            return _productService.GetProducts(pageStart, pageSize, tennent);

        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, $"Exception thrown in {nameof(Get)} type: {e?.GetType()} message: {e?.Message}");

            return new List<ProductDto>();
        }
    }
}