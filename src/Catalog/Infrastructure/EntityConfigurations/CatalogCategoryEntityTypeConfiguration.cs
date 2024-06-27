namespace Catalog.Infrastructure.EntityConfigurations;

public sealed class CatalogCategoryEntityTypeConfiguration : IEntityTypeConfiguration<CatalogCategory>
{
    public void Configure(EntityTypeBuilder<CatalogCategory> builder)
    {
        builder.ToTable(CatalogCategory.TableName);

        builder.HasKey(x => x.Id);

        builder.Ignore(x => x.Path);

        builder.Property(x => x.Category)
               .HasMaxLength(100)
               .IsRequired(true);

        builder.Property(x => x.ParentId)
               .IsRequired(false);

        builder.HasMany(x => x.Children)
               .WithOne(z => z.Parent)
               .HasForeignKey(x => x.ParentId);
    }
}
