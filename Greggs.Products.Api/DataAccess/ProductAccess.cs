using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.DataAccess;

/// <summary>
/// DISCLAIMER: This is only here to help enable the purpose of this exercise, this doesn't reflect the way we work!
/// </summary>
public class ProductAccess : IDataAccess<Product>
{
    private const decimal euro_Exchange_Rate = 1.11m;
    private const string defaultFormat = "C2";

    private static readonly IEnumerable<Product> ProductDatabase = new List<Product>()
    {
        new() { Name = "Sausage Roll", Price = 1m },
        new() { Name = "Vegan Sausage Roll", Price = 1.1m },
        new() { Name = "Steak Bake", Price = 1.2m },
        new() { Name = "Yum Yum", Price = 0.7m },
        new() { Name = "Pink Jammie", Price = 0.5m },
        new() { Name = "Mexican Baguette", Price = 2.1m },
        new() { Name = "Bacon Sandwich", Price = 1.95m },
        new() { Name = "Coca Cola", Price = 1.2m }
    };

    public IEnumerable<Product> List(int? pageStart, int? pageSize, string defaultCurrency = null)
    {
        // clone new List as we don't want to change the actual reference list (incase we search again)
        var cloneList = ProductDatabase.Select(x => new Product(x.Name, x.Price)).ToList();

        if (defaultCurrency != null)
        {
            cloneList.ForEach(x => x.Price = AddExchangeRateToPrice(x.Price, defaultCurrency));
        }        

        var queryable = cloneList.AsQueryable();

        if (pageStart.HasValue)
            queryable = queryable.Skip(pageStart.Value);

        if (pageSize.HasValue)
            queryable = queryable.Take(pageSize.Value);

        return queryable.ToList();
    }

    /// <summary>
    /// Can be used with multiple currency exchange rate
    /// </summary>
    /// <param name="value"></param>
    /// <param name="defaultCurrency"></param>
    /// <returns></returns>
    private decimal AddExchangeRateToPrice(decimal value, string defaultCurrency)
    {
        switch (defaultCurrency?.ToUpper())
        {
            case "EU":            
            case "EUR":
            case "EURO":
                return value * euro_Exchange_Rate;

            default:
                return value;
        }
    }

    // Following can be used to show price with currency symbol
    // by adding string property to the Model i.e. PriceWithCurrencySymbol
    private static string AddExchangeRateToPriceWithSymbol(decimal value, string defaultCurrency)
    {
        switch (defaultCurrency.ToUpper())
        {
            case "EURO":
            case "EUR":
                var price = value * euro_Exchange_Rate;
                return price.ToString(defaultFormat, CultureInfo.CreateSpecificCulture("fr-FR"));

            default:
                return value.ToString(defaultFormat, CultureInfo.CreateSpecificCulture("en-GB"));
        }
    }
}