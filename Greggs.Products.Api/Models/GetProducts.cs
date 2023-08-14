using System.Collections.Generic;

namespace Greggs.Products.Api.Models
{
    public class GetProducts
    {
        public int ProductsFetched { get; set; }
        public string Currency { get; set; }
        public int? PageOffset { get; set; }
        public int? PageSize { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}

