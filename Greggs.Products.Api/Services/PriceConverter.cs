using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using System;
using System.Collections.Generic;

namespace Greggs.Products.Api.Services
{
    public class PriceConverter : IPriceConverter
    {
        private readonly IExchangeRates _exchangeRates;

        public PriceConverter(IExchangeRates exchangeRates)
        {
            _exchangeRates = exchangeRates ?? throw new ArgumentNullException(nameof(exchangeRates));
        }

        public IEnumerable<Product> UpdateProductsWithRequestedCurrency(IEnumerable<Product> products, string isoCurrencyCode)
        {
            var rate = _exchangeRates.Get("GBP", isoCurrencyCode);

            foreach (var product in products)
            {
                product.PriceInRequestedCurrency = new ProductPrice() { IsoCurrencyCode = isoCurrencyCode, Price = Math.Round(product.PriceInPounds * rate, 2) };
            }

            return products;
        }
    }
}
