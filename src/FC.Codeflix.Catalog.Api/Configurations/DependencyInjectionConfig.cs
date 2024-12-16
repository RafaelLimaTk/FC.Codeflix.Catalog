using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Domain.Interfaces;
using FC.Codeflix.Catalog.Infra.Data.EF;
using FC.Codeflix.Catalog.Infra.Data.EF.Repositories;

namespace FC.Codeflix.Catalog.Api.Configurations;

public static class DependencyInjectionConfig
{
    public static void AddRegister(this IServiceCollection services)
    {
        #region Repositories
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        #endregion

        #region UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        #endregion
    }
}
