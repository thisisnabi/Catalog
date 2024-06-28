
namespace Catalog.Apis.Contracts;

public sealed record UpdateCatalogItemRequest(
    int Id,
    string Name,
    string Description,
    int CatalogId,
    int BrandId);

public sealed class UpdateCatalogItemRequestValidator : AbstractValidator<UpdateCatalogItemRequest>
{
    public UpdateCatalogItemRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .NotEmpty()
            .NotNull()
            .MaximumLength(5000);

        RuleFor(x => x.CatalogId)
            .NotNull();

        RuleFor(x => x.BrandId)
            .NotNull();

        RuleFor(x => x.Id)
            .NotNull();
    }
}