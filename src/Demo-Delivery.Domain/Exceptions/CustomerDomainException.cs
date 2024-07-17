using System.Runtime.Serialization;

namespace Demo_Delivery.Domain.Exceptions;

public class CustomerDomainException : BaseDomainException
{
    

    public CustomerDomainException(string message) : base(message)
    {
    }

    public CustomerDomainException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public CustomerDomainException()
    {
    }
}