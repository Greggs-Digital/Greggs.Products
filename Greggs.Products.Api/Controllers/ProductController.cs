using System;
using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Respositories;
using Greggs.Products.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IDataRepository<ProductDTO> _productRepository;
        private readonly IExchangeRateService _exchangeRateService;

        public ProductController(ILogger<ProductController> logger, IDataRepository<ProductDTO> pr, IExchangeRateService ers)
        {
            _logger = logger;
            _productRepository = pr;
            _exchangeRateService = ers;
        }
        
        [HttpGet]
        public IEnumerable<ProductDTO> Get(int pageStart = 0, int pageSize = 5, string currencyId = null)
        {
            _logger.LogInformation($"{nameof(Get)}, pageStart {pageStart}, pageSize {pageSize}, currencyId{currencyId}");

            if (pageStart < 0)
            {
                _logger.LogWarning($"{nameof(Get)}, {nameof(pageStart)} ({pageStart}) < 0, set to 0");
                pageStart = 0;
            }

            if (pageSize < 1)
            {
                _logger.LogWarning($"{nameof(Get)}, {nameof(pageSize)} ({pageSize}) < 1, set to 1");
                pageSize = 1;
            }

            if (currencyId != null)
            {
                try
                {
                    return _productRepository.List(pageStart, pageSize)
                                .Select(p => new ProductDTO
                                {
                                    Name = p.Name,
                                    Price = _exchangeRateService.CalculatePriceInCurrency(p.Price, currencyId)
                                })
                                .ToArray();
                }
                catch (Exception e)
                {
                    _logger.LogError($"{nameof(Get)}, Exception raised, returning empty result", e);
                    return Enumerable.Empty<ProductDTO>();
                }
            }
            else
            {
                return _productRepository.List(pageStart, pageSize).ToArray();
            }
        }
    }
}