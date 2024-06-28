namespace Catalog.Apis.Contracts;

public sealed record CreateCatalogCategoryRequest(string Category, int? ParentId);

public sealed class CreateCatalogCategoryRequestValidator : AbstractValidator<CreateCatalogCategoryRequest>
{
    public CreateCatalogCategoryRequestValidator()
    {
        RuleFor(x => x.Category)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);
    }
}