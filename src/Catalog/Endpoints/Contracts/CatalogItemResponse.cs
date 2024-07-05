namespace Catalog.Endpoints.Contracts;

public sealed record CatalogItemResponse(
    string Name,
    string Slug,
    string Description,
    int BrandId,
    string BrandName,
    int CategoryId,
    string CategoryName,
    decimal Price,
    int AvailableStock,
    int MaxStockThreshold);