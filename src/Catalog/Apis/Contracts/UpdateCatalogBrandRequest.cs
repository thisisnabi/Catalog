namespace Catalog.Apis.Contracts;

public sealed record UpdateCatalogBrandRequest(int Id,string Brand);

public sealed class UpdateCatalogBrandRequestValidator : AbstractValidator<UpdateCatalogBrandRequest>
{
    public UpdateCatalogBrandRequestValidator()
    {
        RuleFor(x => x.Brand)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);

        RuleFor(x => x.Id)
            .NotNull();
    }
}