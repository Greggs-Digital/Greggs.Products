using Greggs.Products.Api.Exceptions;
using System.Collections.Generic;

namespace Greggs.Products.Api.DataAccess
{
    public class ExchangeRateAccess : IExchangeRates
    {
        // Static rates but obviously would be retrieved from some alternative source. 
        // We would also likely not be performing price conversion per country as there is more involved than just a converted price from GBP. 
        // Regional price differences are likely to exist even within the same countries. 
        private static Dictionary<string, decimal> rates = new Dictionary<string, decimal>()
        {
            { "GBPEUR", 1.11m }
        };

        public decimal Get(string sourceIsoCurrencyCode, string destinationIsoCurrencyCode)
        {
            var lookupCode = sourceIsoCurrencyCode.ToUpper().Trim() + destinationIsoCurrencyCode.ToUpper().Trim();

            if(rates.TryGetValue(lookupCode, out var rate))
            {
                return rate;
            }
            else
            {
                throw new CurrencyConversionException($"Exchange rate doesnt exist for pair {lookupCode}");
            }
        }
    }
}
