using System;
using System.Linq;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly IMemoryCache _cache;
        private readonly IDataAccess<Models.Product> _dataAccess;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IDataAccess<Models.Product> dataAccess, IMemoryCache cache, ILogger<ProductService> logger)
        {
            _dataAccess = dataAccess;
            _cache = cache;
            _logger = logger;

        }

        public bool CurrencyExists(string currencyCode)
        {
            if (string.IsNullOrEmpty(currencyCode))
                return false;
            if (string.Equals(currencyCode, "GBP", StringComparison.InvariantCultureIgnoreCase))
                return true;

            var config = _cache.Get<Models.CurrencyConversionConfig>(Models.Constants.CURRENCY_CONVERSION_CONFIG);

            if (config == null || config.CurrencyConversionList == null)
                return false;
            return config.CurrencyConversionList.Any(e => string.Equals(e.Currency, currencyCode, StringComparison.OrdinalIgnoreCase));


        }

        public GetProducts GetProducts(int? pageOffset, int? pageSize, string currency)
        {
            _logger.Log(LogLevel.Debug, $"Called GetProducts() for params  pageStart :{pageOffset}, pageSize : {pageSize}, currencyCode :{currency} ");

            var products = _dataAccess.List(pageOffset, pageSize);
            if (products == null || products.Count() == 0)
            {
                return null;
            }
            var config = _cache.Get<Models.CurrencyConversionConfig>(Models.Constants.CURRENCY_CONVERSION_CONFIG);

            CurrencyConversion currencyConversion = null;

            if (config != null
                && config.CurrencyConversionList != null
                && currency != null
                && !string.Equals(config.BaseCurrency, currency, StringComparison.OrdinalIgnoreCase))
            {
                currencyConversion = config.CurrencyConversionList.FirstOrDefault(e => string.Equals(e.Currency, currency, StringComparison.OrdinalIgnoreCase));
            }

            if (currencyConversion == null)
            {
                return new Models.GetProducts { Currency = "GBP", ProductsFetched = products.Count(), PageOffset = pageOffset, PageSize = pageSize, Products = products };
            }
            else
            {
                return new GetProducts
                {
                    Currency = currency.ToUpper()
                    ,
                    PageOffset = pageOffset
                    ,
                    PageSize = pageSize
                    ,
                    ProductsFetched = products.Count()
                    ,
                    Products = products.Select(e => new Models.Product { Name = e.Name, Price = e.Price * currencyConversion.ConversionRate })
                };
            }
        }
    }
}

