using Greggs.Products.Api.Dtos;

namespace Greggs.Products.Services
{
    public interface IProductService
    {
        IEnumerable<ProductDto> GetProducts(int? pageStart, int? pageSize);
        IEnumerable<ProductDto> GetProducts(int? pageStart, int? pageSize, string tennent);
    }
}