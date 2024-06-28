namespace Catalog.Apis.Contracts;

public sealed record UpdateCatalogCategoryRequest(int Id,string Category);
 
public sealed class UpdateCatalogCategoryRequestValidator : AbstractValidator<UpdateCatalogCategoryRequest>
{
    public UpdateCatalogCategoryRequestValidator()
    {
        RuleFor(x => x.Category)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);

        RuleFor(x => x.Id)
            .NotNull();
    }
}