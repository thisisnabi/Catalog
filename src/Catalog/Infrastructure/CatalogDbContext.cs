using System.Reflection;

namespace Catalog.Infrastructure;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    private const string DefaultSchema = "catalog";
    public const string DefaultConnectionStringName = "SvcDbContext";

    public DbSet<CatalogBrand> CatalogBrands => Set<CatalogBrand>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(DefaultSchema);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
