using System;

namespace Greggs.Products.Api.Services
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private const decimal ExchangeRate = 1.11m;

        public decimal ConvertToEuro(decimal priceInPounds)
        {
            return Math.Round(priceInPounds * ExchangeRate, 2);
        }
    }
}
