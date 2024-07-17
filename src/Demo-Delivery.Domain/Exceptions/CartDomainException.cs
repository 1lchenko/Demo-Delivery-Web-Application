using System.Runtime.Serialization;

namespace Demo_Delivery.Domain.Exceptions;

public class CartDomainException : BaseDomainException
{
    public CartDomainException()
    {
    }

    public CartDomainException(string message) : base(message)
    {
    }

    public CartDomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}