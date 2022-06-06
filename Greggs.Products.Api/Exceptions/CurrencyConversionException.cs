using System;

namespace Greggs.Products.Api.Exceptions
{
    public class CurrencyConversionException : Exception
    {
        public CurrencyConversionException()
        {
        }

        public CurrencyConversionException(string message) : base(message)
        {
        }
    }
}
