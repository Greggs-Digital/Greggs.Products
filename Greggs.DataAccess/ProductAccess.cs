namespace Greggs.DataAccess;

/// <summary>
/// DISCLAIMER: This is only here to help enable the purpose of this exercise, this doesn't reflect the way we work!
/// </summary>
public class ProductAccess : IDataAccess<Product>
{
    private static readonly IEnumerable<Product> ProductDatabase = new List<Product>
    {
        new() { Name = "Sausage Roll", PriceInPounds = 1m, CreatedDate = DateTime.UtcNow.AddHours(-2)},
        new() { Name = "Vegan Sausage Roll", PriceInPounds = 1.1m, CreatedDate = DateTime.UtcNow.AddHours(-26) },
        new() { Name = "Bacon Sandwich", PriceInPounds = 1.95m, CreatedDate = DateTime.UtcNow.AddHours(-25) },
        new() { Name = "Coca Cola", PriceInPounds = 1.2m, CreatedDate = DateTime.UtcNow.AddDays(-4) },
        new() { Name = "Mexican Baguette", PriceInPounds = 2.1m, CreatedDate = DateTime.UtcNow.AddDays(-27) },
        new() { Name = "Steak Bake", PriceInPounds = 1.2m, CreatedDate = DateTime.UtcNow.AddDays(-7) },
        new() { Name = "Yum Yum", PriceInPounds = 0.7m, CreatedDate = DateTime.UtcNow.AddDays(-8) },
        new() { Name = "Pink Jammie", PriceInPounds = 0.5m, CreatedDate = DateTime.UtcNow.AddMonths(-1) }
    };

    public IEnumerable<Product> List(int? pageStart, int? pageSize)
    {
        var queryable = ProductDatabase.AsQueryable();

        if (pageStart.HasValue)
            queryable = queryable.Skip(pageStart.Value);

        if (!pageSize.HasValue) 
            return queryable.ToList();

        if (pageSize > queryable.Count())
        {
            pageSize = queryable.Count();
        }
        queryable = queryable.Take(pageSize.Value);

        return queryable.ToList();
    }

    public IEnumerable<Product> ListOrderByDescending(int? pageStart, int? pageSize, string param)
    {
        var propertyInfo = typeof(Product).GetProperty(param);

        var queryable = ProductDatabase.OrderByDescending(p => propertyInfo?.GetValue(p, null)).AsQueryable();

        if (pageStart.HasValue)
            queryable = queryable.Skip(pageStart.Value);

        if (!pageSize.HasValue)
            return queryable.ToList();

        if (pageSize > queryable.Count())
        {
            pageSize = queryable.Count();
        }
        queryable = queryable.Take(pageSize.Value);

        return queryable.ToList();
    }
}