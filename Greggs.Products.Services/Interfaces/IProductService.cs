namespace Greggs.Products.Services.Interfaces;

public interface IProductService
{
    IEnumerable<Product> GetLatestProducts(int? pageStart, int? pageSize);
}

