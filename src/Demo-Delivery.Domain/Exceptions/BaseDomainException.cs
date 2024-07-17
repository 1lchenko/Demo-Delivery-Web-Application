namespace Demo_Delivery.Domain.Exceptions;

public abstract class BaseDomainException : Exception
{
    private string? _error;

    public BaseDomainException()
    {
    }

    public BaseDomainException(string message) : base(message)
    {
        Error = message;
    }

    public BaseDomainException(string message, Exception innerException) : base(message, innerException)
    {
        Error = message;
    }

    public string Error
    {
        get => _error ?? base.Message;
        set => _error = value;
    }

    public override string Message => _error ?? base.Message;
}