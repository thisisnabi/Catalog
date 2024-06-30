namespace Catalog.Apis;

public static class CatalogItemApi
{
    public static IEndpointRouteBuilder MapCatalogItemApis(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", CreateItem);
        app.MapPut("/", UpdateItem);
        app.MapPatch("/max_stock_threshold", UpdateMaxStockThreshold);
        app.MapDelete("/{id:int:required}", DeleteItemById);
        app.MapGet("/{id:int:required}", GetItemById);
        app.MapGet("/", GetItems);
        app.MapPost("/{id:int:required}/media", UploadMedia);


        return app;
    }

    public static async Task<Results<Created, ValidationProblem, BadRequest<string>>> CreateItem(
        [AsParameters] CatalogServices services,
        CreateCatalogItemRequest itemToCreate,
        IValidator<CreateCatalogItemRequest> validator,
        CancellationToken cancellationToken)
    {
        var validate = validator.Validate(itemToCreate);
        if (!validate.IsValid)
        {
            return TypedResults.ValidationProblem(validate.ToDictionary());
        }

        var hasCategory = await services.Context.CatalogCategories.AnyAsync(x => x.Id == itemToCreate.CatalogId, cancellationToken);
        if (!hasCategory)
        {
            return TypedResults.BadRequest($"A category Id is not valid.");
        }

        var hasBrand = await services.Context.CatalogBrands.AnyAsync(x => x.Id == itemToCreate.BrandId, cancellationToken);
        if (!hasBrand)
        {
            return TypedResults.BadRequest($"A brand Id is not valid.");
        }

        var hasItemSlug = await services.Context.CatalogItems.AnyAsync(x => x.Slug == itemToCreate.Name.ToKebabCase(), cancellationToken);
        if (hasItemSlug)
        {
            return TypedResults.BadRequest($"A Item with the slug '{itemToCreate.Name.ToKebabCase()}' already exists.");
        }

        var item = CatalogItem.Create(
            itemToCreate.Name,
            itemToCreate.Description,
            itemToCreate.MaxStockThreshold,
            itemToCreate.BrandId, itemToCreate.CatalogId);

        services.Context.CatalogItems.Add(item);
        await services.Context.SaveChangesAsync(cancellationToken);

        return TypedResults.Created($"/api/v1/items/{item.Id}");
    }

    public static async Task<Results<Created, ValidationProblem, NotFound<string>, BadRequest<string>>> UpdateItem(
    [AsParameters] CatalogServices services,
    UpdateCatalogItemRequest itemToUpdate,
    IValidator<UpdateCatalogItemRequest> validator,
    CancellationToken cancellationToken)
    {
        var validate = validator.Validate(itemToUpdate);
        if (!validate.IsValid)
        {
            return TypedResults.ValidationProblem(validate.ToDictionary());
        }

        var Item = await services.Context.CatalogItems.FirstOrDefaultAsync(i => i.Id == itemToUpdate.Id, cancellationToken);
        if (Item is null)
        {
            return TypedResults.NotFound($"Item with id {itemToUpdate.Id} not found.");
        }

        var hasCategory = await services.Context.CatalogCategories.AnyAsync(x => x.Id == itemToUpdate.CatalogId, cancellationToken);
        if (!hasCategory)
        {
            return TypedResults.BadRequest($"A category Id is not valid.");
        }

        var hasBrand = await services.Context.CatalogBrands.AnyAsync(x => x.Id == itemToUpdate.BrandId, cancellationToken);
        if (!hasBrand)
        {
            return TypedResults.BadRequest($"A brand Id is not valid.");
        }

        var hasItemSlug = await services.Context.CatalogItems.AnyAsync(x => x.Id != Item.Id &&
                                                                            x.Slug == itemToUpdate.Name.ToKebabCase(), cancellationToken);
        if (hasItemSlug)
        {
            return TypedResults.BadRequest($"A Item with the slug '{itemToUpdate.Name.ToKebabCase()}' already exists.");
        }

        Item.Update(itemToUpdate.Name,
                    itemToUpdate.Description,
                    itemToUpdate.BrandId,
                    itemToUpdate.CatalogId);

        await services.Context.SaveChangesAsync(cancellationToken);

        return TypedResults.Created($"/api/v1/items/{Item.Id}");
    }

    public static async Task<Results<Created, ValidationProblem, NotFound<string>, BadRequest<string>>> UpdateMaxStockThreshold(
    [AsParameters] CatalogServices services,
    UpdateCatalogItemMaxStockThresholdRequest itemToUpdate,
    IValidator<UpdateCatalogItemMaxStockThresholdRequest> validator,
    CancellationToken cancellationToken)
    {
        var validate = validator.Validate(itemToUpdate);
        if (!validate.IsValid)
        {
            return TypedResults.ValidationProblem(validate.ToDictionary());
        }

        var Item = await services.Context.CatalogItems.FirstOrDefaultAsync(i => i.Id == itemToUpdate.Id, cancellationToken);
        if (Item is null)
        {
            return TypedResults.NotFound($"Item with id {itemToUpdate.Id} not found.");
        }

        Item.SetMaxStockThreshold(itemToUpdate.MaxStockThreshold);

        await services.Context.SaveChangesAsync(cancellationToken);

        return TypedResults.Created($"/api/v1/items/{Item.Id}");
    }

    public static async Task<Results<NoContent, NotFound, BadRequest<string>>> DeleteItemById
        ([AsParameters] CatalogServices services, int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return TypedResults.BadRequest("Id is not valid.");
        }

        var item = await services.Context.CatalogItems.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return TypedResults.NotFound();
        }

        services.Context.CatalogItems.Remove(item);
        await services.Context.SaveChangesAsync(cancellationToken);
        return TypedResults.NoContent();
    }

    public static async Task<Results<Ok<CatalogItemResponse>, NotFound, BadRequest<string>>> GetItemById(
    [AsParameters] CatalogServices services,
    int id)
    {
        if (id <= 0)
        {
            return TypedResults.BadRequest("Id is not valid.");
        }

        var item = await services.Context.CatalogItems
                                         .Include(x => x.CatalogBrand)
                                         .Include(x => x.CatalogCategory)
                                         .FirstOrDefaultAsync(ci => ci.Id == id);
        if (item is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(
            new CatalogItemResponse(
                item.Id,
                item.Name,
                item.Slug,
                item.Description,
                item.CatalogBrandId,
                item.CatalogBrand.Brand,
                item.CatalogCategoryId,
                item.CatalogCategory.Category,
                item.Price,
                item.AvailableStock,
                item.MaxStockThreshold));
    }

    public static async Task<Results<Ok<IEnumerable<CatalogItemResponse>>, BadRequest<string>>> GetItems(
    [AsParameters] CatalogServices services,
    CancellationToken cancellationToken)
    {
        var items = await services.Context.CatalogItems
                                          .Include(x => x.CatalogBrand)
                                          .Include(x => x.CatalogCategory)
                                          .Select(x => new CatalogItemResponse(x.Id,
                                                                               x.Name,
                                                                               x.Slug,
                                                                               x.Description,
                                                                               x.CatalogBrandId,
                                                                               x.CatalogBrand.Brand,
                                                                               x.CatalogCategoryId,
                                                                               x.CatalogCategory.Category,
                                                                               x.Price,
                                                                               x.AvailableStock,
                                                                               x.MaxStockThreshold))
                                          .OrderBy(c => c.Id)
                                          .ToListAsync(cancellationToken);

        return TypedResults.Ok<IEnumerable<CatalogItemResponse>>(items);
    }

    // This will be removed in the future. Media as a service
    public static async Task<Results<Ok, BadRequest<string>, NotFound>> UploadMedia(
        int id,
        [AsParameters] CatalogServices services,
        MediaService mediaService,
        HttpRequest request)
    {
        if (!request.HasFormContentType)
        {
            return TypedResults.BadRequest("Expected a form submission.");
        }

        if (id <= 0)
        {
            return TypedResults.BadRequest("Id is not valid.");
        }

        var item = await services.Context.CatalogItems
                                 .Include(x => x.CatalogBrand)
                                 .Include(x => x.CatalogCategory)
                                 .FirstOrDefaultAsync(ci => ci.Id == id);
        if (item is null)
        {
            return TypedResults.NotFound();
        }


        var form = await request.ReadFormAsync();
        var file = form.Files["file"];

        if (file == null || file.Length == 0)
        {
            return TypedResults.BadRequest("File is not selected or is empty.");
        }
         
        using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);

            var url = await mediaService.UploadStream(ms);
            item.AddMedia(file.FileName, url);
        }

        return TypedResults.Ok();
    }
}
