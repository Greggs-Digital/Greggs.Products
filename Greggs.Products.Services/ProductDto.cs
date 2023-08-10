namespace Greggs.Products.Api.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; }
        public decimal PriceInPounds { get; set; }
        public DateTime DateCreated { get; set; }
        public string FormattedPrice { get; set; }
        public Decimal Amount { get; set; }
    }
}
