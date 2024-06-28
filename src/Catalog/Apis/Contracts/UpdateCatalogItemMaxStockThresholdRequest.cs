
namespace Catalog.Apis.Contracts;

public sealed record UpdateCatalogItemMaxStockThresholdRequest(
    int Id,
    int MaxStockThreshold);

public sealed class UpdateCatalogItemMaxStockThresholdRequestValidator : AbstractValidator<UpdateCatalogItemMaxStockThresholdRequest>
{
    public UpdateCatalogItemMaxStockThresholdRequestValidator()
    {
        RuleFor(x => x.MaxStockThreshold)
             .GreaterThan(0);

        RuleFor(x => x.Id)
            .NotNull();
    }
}