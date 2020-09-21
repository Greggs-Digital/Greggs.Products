using System;
namespace Greggs.Products.Api.Services
{
    public interface IExchangeRateService
    {
        /// <summary>
        /// Converts a given price to its equivelant in the currency referenced
        /// Throws ArgumentNullException if currencyId supplied
        /// Throws ArgumentException if the currencyId is not found
        /// Throws ArgumentException if the price in pounds < 0.00 (permits free);
        /// </summary>
        /// <param name="priceInPounds">The price in GBP</param>
        /// <param name="currencyId">The code identifying the currency</param>
        /// <returns></returns>
        public decimal CalculatePriceInCurrency(decimal priceInPounds, string currencyId);
    }
}
