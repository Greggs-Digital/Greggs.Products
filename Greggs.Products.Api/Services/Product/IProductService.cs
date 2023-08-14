namespace Greggs.Products.Api.Services.Product
{
    public interface IProductService
    {
        Models.GetProducts GetProducts(int? pageOffset, int? pageSize, string currency);
        bool CurrencyExists(string currencyCode);
    }
}

