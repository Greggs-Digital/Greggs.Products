using System.Text.Json.Serialization;

namespace Greggs.Products.Api.Models;

public class Product
{
    public string Name { get; set; }
    [JsonIgnore]
    public decimal PriceInPounds { get; set; }
    public decimal Price { get; set; }

}