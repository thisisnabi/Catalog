
namespace Catalog.Apis.Contracts;

public sealed record CreateCatalogBrandRequest(string Brand);

public sealed class CreateCatalogBrandRequestValidator : AbstractValidator<CreateCatalogBrandRequest>
{
    public CreateCatalogBrandRequestValidator()
    {
        RuleFor(x => x.Brand)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);
    }
}