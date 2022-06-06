using Greggs.Products.Api.Models;
using System.Collections.Generic;

namespace Greggs.Products.Api.Services
{
    public interface IPriceConverter
    {
        IEnumerable<Product> UpdateProductsWithRequestedCurrency(IEnumerable<Product> products, string isoCurrencyCode);
    }
}
