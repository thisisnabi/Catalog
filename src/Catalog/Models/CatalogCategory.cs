namespace Catalog.Models;

public class CatalogCategory
{
    public const string TableName = "CatalogCategories";

    public int Id { get; set; }

    public required string Category { get; set; }

    public int? ParentId { get; set; }

    public string Path => GetPath(this);

    private string GetPath(CatalogCategory category)
    {
        if(category.Parent is not null) 
            return GetPath(category.Parent);

        return category.Category;
    }

    public CatalogCategory Parent { get; set; } = null!;

    public ICollection<CatalogCategory> Children { get; set; } = null!;
}
