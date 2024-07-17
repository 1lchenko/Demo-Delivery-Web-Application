namespace Demo_Delivery.Application.Common.Exceptions;

public class AccountException : Exception
{
    public AccountException(string message) : base(message)
    {
    }
}