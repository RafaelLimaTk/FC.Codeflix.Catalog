using FC.Codeflix.Catalog.Infra.Data.EF;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Api.Configurations;

public static class ConnectionsConfig
{
    public static IServiceCollection AddConnections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbConnection();

        return services;
    }

    private static IServiceCollection AddDbConnection(this IServiceCollection services)
    {
        services.AddDbContext<CodeflixCatalogDbContext>(options =>
            options.UseInMemoryDatabase(
                "InMemory-DSV-Database"
            )
        );

        return services;
    }
}
