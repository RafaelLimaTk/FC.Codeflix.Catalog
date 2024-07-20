using FC.Codeflix.Catalog.Domain.Entities;
using FC.Codeflix.Catalog.Infra.Data.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace FC.Codeflix.Catalog.Infra.Data.EF;
public class CodeflixCatalogDbContext : DbContext
{
    public CodeflixCatalogDbContext(
        DbContextOptions<CodeflixCatalogDbContext> options
    )
        : base(options)
    { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Genre> Genres { get; set; }

    public DbSet<GenresCategories> GenresCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CodeflixCatalogDbContext).Assembly);
    }
}