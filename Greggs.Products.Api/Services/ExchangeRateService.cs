using System;
using System.Collections.Generic;

namespace Greggs.Products.Api.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private IDictionary<string, decimal> _exchangeRates;

        public ExchangeRateService()
        {
            _exchangeRates = new Dictionary<string, decimal>();
            _exchangeRates.Add("eur", 1.11m);
        }

        private decimal GetExchangeRate(string currencyId)
        {
            if (_exchangeRates.ContainsKey(currencyId))
            {
                _exchangeRates.TryGetValue(currencyId, out var exchangeRate);
                return exchangeRate;
            }
            else
            {
                throw new ArgumentException($"Currency with id '{currencyId}' not found");
            }
        }

        public decimal CalculatePriceInCurrency(decimal priceInPounds, string currencyId)
        {
            if (string.IsNullOrWhiteSpace(currencyId))
            {
                throw new ArgumentNullException(nameof(currencyId));
            }

            if (priceInPounds < 0.00m)
            {
                throw new ArgumentException($"{nameof(CalculatePriceInCurrency)} priceInPounds < 0.00 found");
            }

            return priceInPounds * GetExchangeRate(currencyId); 
        }
    }
}