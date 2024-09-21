using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interface;
using ProductApi.Infrastucture.Data;
using ProductApi.Infrastucture.Repositories;
using ProductCatalog.SharedLibrary.DependencyInjection;

namespace ProductApi.Infrastucture.DependencyInjection
{
    public static class ContainerServices
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            //Add DB Connection and Add authentication 
            ServiceContainer.AddServices<ProductDbContext>(services, config, config["MySerilog:FineName"]!);

            //Create Dependency Injection between Interface and Repo
            services.AddScoped<IProduct, ProductRepositoty>();
            return services;

        }
        //Adding middelware pipeling
        public static IApplicationBuilder UseInfrastructureService(this IApplicationBuilder app)
        {

            //register middelwares
            ServiceContainer.UsePolicies(app);
            return app;
        }
    }
}