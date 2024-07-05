
namespace Catalog.Endpoints.Contracts;

public sealed record UpdateCatalogItemMaxStockThresholdRequest(
    string Slug,
    int MaxStockThreshold);

public sealed class UpdateCatalogItemMaxStockThresholdRequestValidator : AbstractValidator<UpdateCatalogItemMaxStockThresholdRequest>
{
    public UpdateCatalogItemMaxStockThresholdRequestValidator()
    {
        RuleFor(x => x.MaxStockThreshold)
             .GreaterThan(0);

        RuleFor(x => x.Slug)
            .MaximumLength(150)
            .NotNull();
    }
}