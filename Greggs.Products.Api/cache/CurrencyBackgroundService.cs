using System;
using Microsoft.Extensions.Caching.Memory;
using Greggs.Products.Api.Services.Configuration;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.cache
{
    public class CurrencyBackgroundService : BackgroundService
    {

        private readonly IMemoryCache _cache;
        private readonly ICurrencyConversionConfig _currencyProvider;
        private readonly ILogger<CurrencyBackgroundService> _logger;
        private Timer _timer;

        public CurrencyBackgroundService(IMemoryCache cache, ICurrencyConversionConfig currencyProvider, ILogger<CurrencyBackgroundService> logger)
        {
            _cache = cache;
            _currencyProvider = currencyProvider;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(3));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            InitializeCurrencyConversionCache();
        }

        private void InitializeCurrencyConversionCache()
        {
            _logger.Log(LogLevel.Information, $"Invoked currencyconversioncache () at UTC time {DateTime.UtcNow}");
            var config = _currencyProvider.GetCurrencyConversionConfig();
            if (_cache.Get<Models.CurrencyConversionConfig>(Models.Constants.CURRENCY_CONVERSION_CONFIG) == null)
            {
                _cache.Set<Models.CurrencyConversionConfig>(Models.Constants.CURRENCY_CONVERSION_CONFIG, new Models.CurrencyConversionConfig());
            }

            if (config != null)
            {
                _cache.Set<Models.CurrencyConversionConfig>(Models.Constants.CURRENCY_CONVERSION_CONFIG, config);
            }
        }
    }
}

