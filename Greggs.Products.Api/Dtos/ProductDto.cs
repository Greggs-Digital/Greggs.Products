namespace Greggs.Products.Api.Models;

public class ProductDto
{
    public DateTime CreatedDate { get; set; }

    public string Name { get; set; }

    public decimal PriceInPounds { get; set; }
}