using Greggs.Products.Abstractions;
using Greggs.Products.Api.Dtos;
using Greggs.Products.Entities;
using System.Globalization;

namespace Greggs.Products.Services
{
    public class ProductService : IProductService
    {
        //have static class for this.
        private const string International = "fr";
        private readonly IDataAccess<Product> _productDataAccess;
        private  const decimal ExchangeRate = 1.11m;
        public ProductService(IDataAccess<Product> productDataAccess)
        {
            _productDataAccess = productDataAccess;
        }


        public IEnumerable<ProductDto> GetProducts(int? pageStart, int? pageSize)
        {
            //async and non async mehtods.
            var result = _productDataAccess.List(pageStart, pageSize);

            if (result.Any())
            {

                return result.Select(x => new ProductDto() { Name = x?.Name, PriceInPounds = x.PriceInPounds });
            }


            return new List<ProductDto>();
        }

        public IEnumerable<ProductDto> GetProducts(int? pageStart, int? pageSize, string tennent)
        {

            if (tennent == International)
            {
                var results = _productDataAccess.List(pageStart, pageSize, tennent);
                var cultureInfo = CultureInfo.GetCultureInfo("fr-FR");
                return  results.Select(x => new ProductDto() { Name = x?.Name, Amount = x.PriceInPounds.Convert(ExchangeRate), FormattedPrice = String.Format(cultureInfo, "{0:C}",x.PriceInPounds.Convert(ExchangeRate))});
            }
            else
            {
                return GetProducts(pageStart, pageSize);
            }

        }
    }
}
