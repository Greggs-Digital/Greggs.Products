using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Greggs.Products.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Greggs.Products.Api;

public class Startup
{


    public Startup(IConfiguration configuration)
    {
        Configuration = (IConfigurationRoot)configuration;
    }

    public IConfigurationRoot Configuration { get; }

    public void ConfigureServices(IServiceCollection services)

    {
        services.AddControllers();

        services.AddSwaggerGen();

        // Register the data access layer for products.
        // This line specifies that a single instance of ProductAccess.
        // Will be created and shared throughout the application.
        services.AddSingleton<IDataAccess<Product>, ProductAccess>();

        // Providing a scoped service for currency conversion.
        services.AddScoped<ICurrencyConverterService, CurrencyConverterService>();


        //services.Configure<CurrencyRatesConfig>(Configuration.GetSection("CurrencyRates"));
        services.Configure<CurrencyRatesConfig>(Configuration.GetSection("CurrencyRate"));


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