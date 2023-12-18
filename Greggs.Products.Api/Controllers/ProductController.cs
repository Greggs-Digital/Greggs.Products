using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using System.Linq;

namespace Greggs.Products.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IDataAccess<Product> _dataAccess;
        private readonly ICurrencyConverterService _currencyConverterService;

        public ProductController(
            ILogger<ProductController> logger,
            IDataAccess<Product> dataAccess,
            ICurrencyConverterService currencyConverterService)
        {
            _logger = logger;
            _dataAccess = dataAccess;
            _currencyConverterService = currencyConverterService;
        }

        // GET endpoint to retrieve a list of products.
        // Defaults to returning the first 5 products.
        [HttpGet]
        public IEnumerable<Product> Get(int pageStart = 0, int pageSize = 5, string currency = "GBP")
        {
            var products = _dataAccess.List(pageStart, pageSize).ToList();

            // Convert prices using the currency converter service
            products.ForEach(product =>
            {
                product.Price = currency != "GBP"
                    ? _currencyConverterService.ConvertCurrency(product.Price, currency)
                    : product.Price;
            });

            return products;
        }


    }
}
