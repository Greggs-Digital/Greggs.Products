namespace Greggs.Products.Services;

public class ProductService : IProductService
{
    private readonly IDataAccess<Product> _productAccess;

    public ProductService(IDataAccess<Product> productAccess)
    {
        _productAccess = productAccess;
    }

    public IEnumerable<Product> GetLatestProducts(int? pageStart, int? pageSize)
    {
        return _productAccess.ListOrderByDescending(pageStart, pageSize, "CreatedDate");
    }
}
