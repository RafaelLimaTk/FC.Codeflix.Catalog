using FC.Codeflix.Catalog.Application.Utils;

namespace FC.Codeflix.Catalog.Api.Configurations;

public static class ApiConfig
{
    public static void AddConfigureApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConnections(configuration);
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddSwaggerConfiguration();
        services.AddRegister();
        services.AddUseCases();
        services.AddEndpointsApiExplorer();
    }

    public static void UseConfigureApi(this WebApplication app, IConfiguration configuration)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerConfiguration();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}
