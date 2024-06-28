namespace Catalog.Apis;

public static class CatalogCategoryApi
{
    public static IEndpointRouteBuilder MapCatalogCategoryApis(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", CreateCategory);
        app.MapPut("/", UpdateCategory);
        app.MapDelete("/{id:int:required}", DeleteCategoryById);
        app.MapGet("/{id:int:required}", GetCategoryById);
        app.MapGet("/", GetCategories);

        return app;
    }

    public static async Task<Results<Created, ValidationProblem, BadRequest<string>>> CreateCategory(
        [AsParameters] CatalogServices services,
        CreateCatalogCategoryRequest categoryToCreate,
        IValidator<CreateCatalogCategoryRequest> validator,
        CancellationToken cancellationToken)
    {
        var validate = validator.Validate(categoryToCreate);
        if (!validate.IsValid)
        {
            return TypedResults.ValidationProblem(validate.ToDictionary());
        }

        if (categoryToCreate.ParentId.HasValue)
        {
            var hasParent = await services.Context.CatalogCategories.AnyAsync(x => x.Id == categoryToCreate.ParentId, cancellationToken);
            if (!hasParent)
            {
                return TypedResults.BadRequest($"A parent Id is not valid.");
            }
        }

        var hasCategory = await services.Context.CatalogCategories.AnyAsync(x => x.Category == categoryToCreate.Category &&
                                                                                 x.ParentId == categoryToCreate.ParentId,
                                                                                 cancellationToken);
        if (hasCategory)
        {
            return TypedResults.BadRequest($"A Category with the name '{categoryToCreate.Category}' in this level already exists.");
        }

        var category = CatalogCategory.Create(categoryToCreate.Category, categoryToCreate.ParentId);

        services.Context.CatalogCategories.Add(category);
        await services.Context.SaveChangesAsync(cancellationToken);

        return TypedResults.Created($"/api/v1/categories/{category.Id}");
    }

    public static async Task<Results<Created, ValidationProblem, NotFound<string>>> UpdateCategory(
    [AsParameters] CatalogServices services,
    UpdateCatalogCategoryRequest categoryToUpdate,
    IValidator<UpdateCatalogCategoryRequest> validator,
    CancellationToken cancellationToken)
    {
        var validate = validator.Validate(categoryToUpdate);
        if (!validate.IsValid)
        {
            return TypedResults.ValidationProblem(validate.ToDictionary());
        }

        var category = await services.Context.CatalogCategories.FirstOrDefaultAsync(i => i.Id == categoryToUpdate.Id, cancellationToken);
        if (category is null)
        {
            return TypedResults.NotFound($"Category with id {categoryToUpdate.Id} not found.");
        }

        category.Update(categoryToUpdate.Category);
        await services.Context.SaveChangesAsync(cancellationToken);

        return TypedResults.Created($"/api/v1/categories/{category.Id}");
    }

    public static async Task<Results<NoContent, NotFound, BadRequest<string>>> DeleteCategoryById
        ([AsParameters] CatalogServices services, int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return TypedResults.BadRequest("Id is not valid.");
        }

        var category = await services.Context.CatalogCategories.FirstOrDefaultAsync(x => x.Id == id);
        if (category is null)
        {
            return TypedResults.NotFound();
        }

        services.Context.CatalogCategories.Remove(category);
        await services.Context.SaveChangesAsync(cancellationToken);
        return TypedResults.NoContent();
    }

    public static async Task<Results<Ok<CatalogCategoryResponse>, NotFound, BadRequest<string>>> GetCategoryById(
    [AsParameters] CatalogServices services,
    int id)
    {
        if (id <= 0)
        {
            return TypedResults.BadRequest("Id is not valid.");
        }

        var category = await services.Context.CatalogCategories.FirstOrDefaultAsync(ci => ci.Id == id);
        if (category is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(new CatalogCategoryResponse(id, category.Category,category.Path));
    }

    public static async Task<Results<Ok<IEnumerable<CatalogCategoryResponse>>, BadRequest<string>>> GetCategories(
    [AsParameters] CatalogServices services,
    CancellationToken cancellationToken)
    {
        var categories = await services.Context.CatalogCategories
                                       .OrderBy(c => c.Id)
                                       .Select(x => new CatalogCategoryResponse(x.Id, x.Category, x.Path))
                                       .ToListAsync(cancellationToken);

        return TypedResults.Ok<IEnumerable<CatalogCategoryResponse>>(categories);
    }
}
