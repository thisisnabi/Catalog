namespace Catalog.Apis;

public static class CatalogCategoryApi
{
    public static IEndpointRouteBuilder MapCatalogCategoryApis(this IEndpointRouteBuilder app)
    {
        //app.MapPost("/", CreateBrand);
        //app.MapPut("/", UpdateBrand);
        //app.MapDelete("/{id:int}", DeleteBrandById);
        //app.MapGet("/", GetBrands);

        //app.MapPut("/items", UpdateItem);
        //app.MapPost("/items", CreateItem);
        //app.MapDelete("/items/{id:int}", DeleteItemById);



        //// Routes for querying catalog items.
        //app.MapGet("/items", GetAllItems);
        //app.MapGet("/items/by", GetItemsByIds);
        //app.MapGet("/items/{id:int}", GetItemById);
        //app.MapGet("/items/by/{name:minlength(1)}", GetItemsByName);
        //app.MapGet("/items/{catalogItemId:int}/pic", GetItemPictureById);

        //// Routes for resolving catalog items using AI.
        //app.MapGet("/items/withsemanticrelevance/{text:minlength(1)}", GetItemsBySemanticRelevance);

        //// Routes for resolving catalog items by type and brand.
        //app.MapGet("/items/type/{typeId}/brand/{brandId?}", GetItemsByBrandAndTypeId);
        //app.MapGet("/items/type/all/brand/{brandId:int?}", GetItemsByBrandId);
        //app.MapGet("/catalogtypes", async (CatalogContext context) => await context.CatalogTypes.OrderBy(x => x.Type).ToListAsync());
        //app.MapGet("/catalogbrands", async (CatalogContext context) => await context.CatalogBrands.OrderBy(x => x.Brand).ToListAsync());

        // Routes for modifying catalog items.
        //app.MapPut("/items", UpdateItem);
        //app.MapPost("/items", CreateItem);
        //app.MapDelete("/items/{id:int}", DeleteItemById);

        return app;
    }
}
