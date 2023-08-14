using Greggs.Products.Api.cache;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Services.Configuration;
using Greggs.Products.Api.Services.Product;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Greggs.Products.Api;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IDataAccess<Models.Product>, ProductAccess>();
        services.AddSingleton<ICurrencyConversionConfig, CurrencyConversionService>();

        services.AddScoped<IProductService, ProductService>();
        services.AddMemoryCache();
        services.AddHostedService<CurrencyBackgroundService>();
        services.AddControllers();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Greggs Products API V1"); });

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}