using Microsoft.Extensions.DependencyInjection;

namespace FC.Codeflix.Catalog.Application.Utils;
public static class UseCasesConfiguration
{
    public static IServiceCollection AddUseCases(
        this IServiceCollection services)
    {
        services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(UseCasesConfiguration).Assembly));

        return services;
    }
}
