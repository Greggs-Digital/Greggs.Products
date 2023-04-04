using System;
using System.Collections.Generic;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IDataAccess<Product> _dataAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductController"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    /// <param name="dataAccess">The data access.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ProductController(
        ILogger<ProductController> logger,
        IDataAccess<Product> dataAccess)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(dataAccess);

        _logger = logger;
        _dataAccess = dataAccess;
    }

    /// <summary>
    /// Gets a page of products from <see cref="IDataAccess{T}"/> of <see cref="Product"/>.
    /// </summary>
    /// <param name="pageStart">The page start.</param>
    /// <param name="pageSize">Size of the page.</param>
    /// <returns>A sequence of <see cref="Product"/>.</returns>
    [HttpGet]
    public IEnumerable<Product> Get(int pageStart = 0, int pageSize = 5)
    {
        _logger.LogTrace($"Begin call to {nameof(ProductController.Get)}");
        if (pageSize < 0) throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, null);
        if (pageStart < 0) throw new ArgumentOutOfRangeException(nameof(pageStart), pageStart, null);

        // An Assumption here about the ordering of the products.
        // Some concern that this is not asynchronous.
        return _dataAccess.List(pageStart, pageSize);
    }
}