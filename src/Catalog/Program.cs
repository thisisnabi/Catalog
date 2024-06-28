using Catalog.Apis;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGroup("/api/v1/brands")
   .WithTags("Brand APIs")
   .MapCatalogBrandApis();

app.MapGroup("/api/v1/categories")
   .WithTags("Category APIs")
   .MapCatalogCategoryApis();


app.Run();