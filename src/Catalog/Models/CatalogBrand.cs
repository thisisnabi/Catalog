namespace Catalog.Models;

public class CatalogBrand
{
    public const string TableName = "CatalogBrands";

    public int Id { get; set; }

    public required string Brand { get; set; }
}
