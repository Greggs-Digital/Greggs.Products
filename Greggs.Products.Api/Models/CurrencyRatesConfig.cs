using System.Collections.Generic;

namespace Greggs.Products.Api.Models
{
    public class CurrencyRatesConfig
    {
        public string BaseCurrency { get; set; }
        public List<CurrencyRate> CurrencyList { get; set; }
    }

    public class CurrencyRate
    {
        public string Currency { get; set; }
        public decimal Rate { get; set; }
    }
}

