using System;
using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.Respositories
{
    public class ProductRepository : IDataRepository<ProductDTO>
    {
        private readonly IDataAccess<Product> _productDataAccess;

        public ProductRepository(IDataAccess<Product> dataAccess)
        {
            _productDataAccess = dataAccess;
        }

        public IEnumerable<ProductDTO> List(int? pageStart, int? pageSize)
        {
            return _productDataAccess.List(pageStart, pageSize)
                                     .Select(p => new ProductDTO
                                     {
                                        Name = p.Name,
                                        Price = p.PriceInPounds
                                     });
        }
    }
}