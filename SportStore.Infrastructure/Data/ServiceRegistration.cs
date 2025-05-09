using Microsoft.Extensions.DependencyInjection;
using SportStore.Application;

namespace SportStore.Infrastructure.Data;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        return services;
    }
} 