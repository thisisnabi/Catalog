

namespace Catalog.Models;

public class CatalogCategory
{
    public const string TableName = "CatalogCategories";

    public int Id { get; private set; }

    public string Category { get; private set; } = null!;

    public int? ParentId { get; private set; }

    public string? Path => GetPath(this);

    private string? GetPath(CatalogCategory category)
    {
        if (category.Parent is not null)
            return $"{GetPath(category.Parent)}/{category.Category}";

        if(category.Id == Id)
            return null;

        return category.Category;
    }

    public static CatalogCategory Create(string category, int? parentId) 
        => new CatalogCategory
        {
            Category = category,
            ParentId = parentId
        };

    public void Update(string category)
    {
        Category = category;
    }

    public CatalogCategory Parent { get; private set; } = null!;

    public ICollection<CatalogCategory> Children { get; private set; } = null!;
}
