namespace Greggs.Products.Api.Services
{
    public interface ICurrencyConverterService
    {
        decimal ConvertToEuro(decimal priceInPounds);
    }
}
