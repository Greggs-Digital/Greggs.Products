using System.Collections.Generic;
using System.Linq;
using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.DataAccess
{
    public class ProductAccess : IDataAccess<Product>
    {
        private static readonly IEnumerable<Product> ProductDatabase = new List<Product>()
        {
            new() { Name = "Sausage Roll", PriceInPounds = 1m },
            new() { Name = "Vegan Sausage Roll", PriceInPounds = 1.1m },
            new() { Name = "Steak Bake", PriceInPounds = 1.2m },
            new() { Name = "Yum Yum", PriceInPounds = 0.7m },
            new() { Name = "Pink Jammie", PriceInPounds = 0.5m },
            new() { Name = "Mexican Baguette", PriceInPounds = 2.1m },
            new() { Name = "Bacon Sandwich", PriceInPounds = 1.95m },
            new() { Name = "Coca Cola", PriceInPounds = 1.2m }
        };

        public IEnumerable<Product> List(int? pageStart, int? pageSize)
        {
            var queryable = ProductDatabase.AsQueryable();

            if (pageStart.HasValue)
                queryable = queryable.Skip(pageStart.Value);

            if (pageSize.HasValue)
                queryable = queryable.Take(pageSize.Value);

            return MapToModel(queryable.ToList());
        }

        private IEnumerable<Product> MapToModel(IEnumerable<Product> products)
        {
            return products.Select(product => new Product
            {
                Name = product.Name,
                Price = product.PriceInPounds, 
            });
        }
    }
}
