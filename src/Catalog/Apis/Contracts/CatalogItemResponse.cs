namespace Catalog.Apis.Contracts;

public sealed record CatalogItemResponse(
    int Id,
    string Name,
    string Slug,
    string Description,
    int BrandId,
    string BrandName,
    int CategoryId,
    string CategoryName,
    decimal Price,
    int AvailableStock,
    int MaxStockThreshold
    );