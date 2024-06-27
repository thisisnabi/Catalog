namespace Catalog.Infrastructure.Exceptions;

public sealed class PriceGreaterThanZeroException : CatalogDomainException
{
    private const string _message = "Item price desired should be greater than zero";

    public PriceGreaterThanZeroException() : base(_message)
    {
        
    }
}
