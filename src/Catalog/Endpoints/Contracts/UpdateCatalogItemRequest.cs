namespace Catalog.Endpoints.Contracts;

public sealed record UpdateCatalogItemRequest(
    string slug,
    string Description,
    int CatalogId,
    int BrandId);

public sealed class UpdateCatalogItemRequestValidator : AbstractValidator<UpdateCatalogItemRequest>
{
    public UpdateCatalogItemRequestValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull()
            .MaximumLength(5000);

        RuleFor(x => x.CatalogId)
            .NotNull();

        RuleFor(x => x.BrandId)
            .NotNull();

        RuleFor(x => x.slug)
            .MaximumLength(150)
            .NotNull();
    }
}