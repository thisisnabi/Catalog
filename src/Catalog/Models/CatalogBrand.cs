namespace Catalog.Models;

public class CatalogBrand
{
    public const string TableName = "CatalogBrands";

    public int Id { get; set; }

    public string Brand { get; private set; } = null!;

    public void Update(string brand) => Brand = brand;

    public static CatalogBrand Create(string brand)
        => new CatalogBrand
        {
            Brand = brand
        };
}
