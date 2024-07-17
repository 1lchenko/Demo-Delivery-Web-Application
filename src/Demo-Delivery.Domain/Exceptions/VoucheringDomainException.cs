using System.Runtime.Serialization;

namespace Demo_Delivery.Domain.Exceptions;

public class VoucheringDomainException : BaseDomainException
{
    public VoucheringDomainException()
    {
    }

    public VoucheringDomainException(string message) : base(message)
    {
    }

    public VoucheringDomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}