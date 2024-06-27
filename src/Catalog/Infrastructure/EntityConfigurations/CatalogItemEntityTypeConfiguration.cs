namespace Catalog.Infrastructure.EntityConfigurations;

public sealed class CatalogItemEntityTypeConfiguration : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.ToTable(CatalogItem.TableName);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.Description)
               .IsRequired()
               .HasMaxLength(5000);

        builder.Property(x => x.Slug)
               .IsRequired()
               .HasMaxLength(150);

        builder.HasIndex(x => x.Slug);
    }
}
