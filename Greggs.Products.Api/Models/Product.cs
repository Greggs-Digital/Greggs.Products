namespace Greggs.Products.Api.Models;

public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Product()
    {
    }

    /// <summary>
    /// Using for the cloning wihout having actual reference
    /// </summary>
    /// <param name="name"></param>
    /// <param name="price"></param>
    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}