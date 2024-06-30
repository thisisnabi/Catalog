
namespace Catalog.Models;

public class CatalogItem
{
    public const string TableName = "CatalogItems";

    public int Id { get; private set; }

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public decimal Price { get; private set; }

    public int AvailableStock { get; private set; }

    public string Slug { get; private set; } = null!;

    public int MaxStockThreshold { get; private set; }

    public ICollection<CatalogMedia> Medias { get; private set; } = [];

    public static CatalogItem Create(string name, string description, int maxStockThreshold, int brandId, int categoryId, decimal price = default)
    {
        var newItem = new CatalogItem
        {
            Name = name,
            CatalogBrandId = brandId,
            CatalogCategoryId = categoryId,
            Description = description,
            Slug = name.ToKebabCase(),
            Price = price
        };

        newItem.SetMaxStockThreshold(maxStockThreshold);

        return newItem;
    }

    public void Update(string name, string description, int brandId, int categoryId)
    {
        Name = name;
        CatalogBrandId = brandId;
        CatalogCategoryId = categoryId;
        Description = description;
        Slug = name.ToKebabCase();
    }
    public void SetMaxStockThreshold(int maxStockThreshold)
    {
        if (maxStockThreshold <= 0)
            throw new PriceGreaterThanZeroException();

        MaxStockThreshold = maxStockThreshold;
    }

    public CatalogBrand CatalogBrand { get; private set; } = null!;

    public int CatalogBrandId { get; private set; }

    public CatalogCategory CatalogCategory { get; private set; } = null!;

    public int CatalogCategoryId { get; private set; }

    public int RemoveStock(int quantity)
    {
        if (AvailableStock == 0)
        {
            throw new EmptyStockException(Name);
        }

        if (quantity <= 0)
        {
            throw new QuantityGreaterThanZeroException();
        }

        int removed = Math.Min(quantity, AvailableStock);

        AvailableStock -= removed;
        return removed;
    }

    public int AddStock(int quantity)
    {
        int original = AvailableStock;

        if ((AvailableStock + quantity) > MaxStockThreshold)
        {
            AvailableStock += (MaxStockThreshold - AvailableStock);
        }
        else
        {
            AvailableStock += quantity;
        }

        return AvailableStock - original;
    }

    public void UpdatePrice(decimal price)
    {
        if (price <= 0)
            throw new PriceGreaterThanZeroException();

        Price = price;
    }

    public void AddMedia(string fileName, string url)
    {
        Medias.Add(new CatalogMedia(fileName, url));
    }
}
