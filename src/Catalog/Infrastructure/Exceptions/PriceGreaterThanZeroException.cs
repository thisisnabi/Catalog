namespace Catalog.Infrastructure.Exceptions;

public sealed class MaxStockThresholdGreaterThanZeroException : CatalogDomainException
{
    private const string _message = "Item max stock threshold desired should be greater than zero";

    public MaxStockThresholdGreaterThanZeroException() : base(_message)
    {
        
    }
}
