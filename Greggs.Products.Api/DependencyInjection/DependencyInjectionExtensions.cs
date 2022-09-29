namespace Greggs.Products.Api.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static void RegisterDataAccessDependencies(this IServiceCollection services)
    {
        services.AddScoped<IDataAccess<Product>>(_ => new ProductAccess());
        services.AddScoped<IProductService, ProductService>();
    }
}

