using System.Collections.Generic;

namespace Greggs.Products.Api.Models
{
    public class CurrencyConversionConfig
    {
        public string BaseCurrency { get; set; }
        public IEnumerable<CurrencyConversion> CurrencyConversionList { get; set; }

    }
    public class CurrencyConversion
    {
        public string Currency { get; set; }
        public decimal ConversionRate { get; set; }
    }
}

