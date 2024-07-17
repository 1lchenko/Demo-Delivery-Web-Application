using System.Runtime.Serialization;

namespace Demo_Delivery.Domain.Exceptions;

public class OrderingDomainException : BaseDomainException
{
    public OrderingDomainException()
    {
    }

    public OrderingDomainException(string message) : base(message)
    {
    }

    public OrderingDomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}