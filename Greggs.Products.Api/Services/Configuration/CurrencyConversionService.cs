using Microsoft.Extensions.Configuration;
using Greggs.Products.Api.Models;
using System.Linq;

namespace Greggs.Products.Api.Services.Configuration
{
    public class CurrencyConversionService : ICurrencyConversionConfig
    {
        private readonly IConfiguration _configuration;

        public CurrencyConversionService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public CurrencyConversionConfig GetCurrencyConversionConfig()
        {
            var config = new CurrencyConversionConfig();
            _configuration.GetSection(Constants.CURRENCY_CONVERSION_CONFIG).Bind(config);

            if (config == null)
                config = new CurrencyConversionConfig();
            else
                FilterImproperData(config);
            return config;

        }


        private void FilterImproperData(CurrencyConversionConfig config)
        {
            if (config == null || config.CurrencyConversionList == null)
            {
                return;
            }
            config.CurrencyConversionList = config.CurrencyConversionList.Where(c => c.Currency.Length == 3 && c.ConversionRate > 0);
        }

    }
}

