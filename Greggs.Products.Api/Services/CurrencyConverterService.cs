using System;
using Microsoft.Extensions.Options;
using System.Linq;
using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.Services
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private readonly CurrencyRatesConfig _config;

        public CurrencyConverterService(IOptions<CurrencyRatesConfig> config)
        {
            _config = config.Value;
        }

        public decimal ConvertCurrency(decimal priceInPounds, string currencyCode)
        {
            var exchangeRate = GetExchangeRate(currencyCode.ToUpper());
            return Math.Round(priceInPounds * exchangeRate, 2);
        }

        private decimal GetExchangeRate(string currencyCode)
        {
            if (currencyCode == _config.BaseCurrency)
            {
                return 1; // The base currency has an exchange rate of 1
            }

            var currencyRate = _config.CurrencyList
                .FirstOrDefault(c => c.Currency == currencyCode)?.Rate;

            if (currencyRate.HasValue)
            {
                return currencyRate.Value;
            }

            throw new InvalidOperationException($"Exchange rate not found for {currencyCode}");
        }
    }

}
