namespace Greggs.Products.Api.DataAccess
{
    public interface IExchangeRates
    {
        decimal Get(string sourceIsoCurrencyCode, string destinationIsoCurrencyCode);
    }
}
