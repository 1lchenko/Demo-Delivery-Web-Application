namespace Demo_Delivery.Domain.Exceptions;

public class CatalogDomainException : BaseDomainException
{
    public CatalogDomainException() 
    {
    }

    public CatalogDomainException(string message) : base(message)
    {
    }

    public CatalogDomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}