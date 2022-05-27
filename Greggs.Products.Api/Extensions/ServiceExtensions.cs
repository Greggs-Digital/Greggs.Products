using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Greggs.Products.Api.Extensions;

public static class ServiceExtensions
{
    public static void AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IDataAccess<Product>, ProductAccess>();
    }
}

