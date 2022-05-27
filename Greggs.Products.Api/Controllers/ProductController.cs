using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greggs.Products.Api.Mediator;
using Greggs.Products.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    //private static readonly string[] Products = new[]
    //{
    //    "Sausage Roll", "Vegan Sausage Roll", "Steak Bake", "Yum Yum", "Pink Jammie"
    //};

    private readonly ILogger<ProductController> _logger;
    private readonly IMediator _mediator;

    public ProductController(ILogger<ProductController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageStart"></param>
    /// <param name="pageSize"></param>
    /// <param name="defaultCurrency">use 'Euro' or 'EU' or 'EUR' to get Euro prices OR null for GBP prices</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult> Get(int pageStart = 0, int pageSize = 10, string defaultCurrency = null)
    {
        var request = new GetProductRequest
        {
            PageStart = pageStart,
            PageSize = pageSize,
            DefaultCurrency = defaultCurrency
        };

        if (_mediator is null)
        {
            _logger.LogInformation(message: $"ProductController:Get(pageStart={pageStart}, pageSize={pageSize}, defaultCurrency={defaultCurrency})");
            return Ok(Enumerable.Empty<Product>());
        }

        var response = await _mediator.Send(request).ConfigureAwait(false);

        return Ok(response);
    }
}