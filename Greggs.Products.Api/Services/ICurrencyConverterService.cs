namespace Greggs.Products.Api.Services
{
    public interface ICurrencyConverterService
    {
        decimal ConvertCurrency(decimal priceInPounds,string currencyCode);
    }
}
