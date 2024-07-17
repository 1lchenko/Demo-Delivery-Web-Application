using Demo_Delivery.Domain.Exceptions;

namespace Demo_Delivery.Domain.SeedWork;

public static class Guard
{
     public static void AgainstEmptyString<TException>(string value, string name = "Value")
        where TException : BaseDomainException, new()
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        ThrowException<TException>($"{name} cannot be null or empty.");
    }

    public static void ForStringLength<TException>(string value, int minLength, int maxLength, string name = "Value")
        where TException : BaseDomainException, new()
    {
        AgainstEmptyString<TException>(value, name);

        if (minLength <= value.Length && value.Length <= maxLength)
        {
            return;
        }

        ThrowException<TException>($"{name} must have between {minLength} and {maxLength} symbols.");
    }

    public static void AgainstOutOfRange<TException>(int number, int min, int max, string name = "Value")
        where TException : BaseDomainException, new()
    {
        if (min <= number && number <= max)
        {
            return;
        }

        ThrowException<TException>($"{name} must be between {min} and {max}.");
    }
    
    public static void AgainstOutOfDateTimeRange<TException>(DateTime dateValue, DateTime dateFrom, DateTime dateTo)
        where TException : BaseDomainException, new()
    {
        if (dateFrom <= dateValue && dateValue <= dateTo)
        {
            return;
        }

        ThrowException<TException>($"Date: {dateValue} must be great than Date From: {dateFrom} and less that {dateTo}");
    }
    
    public static void AgainstOutOfDateRangeBetweenTwo<TException>(DateTime dateFrom, DateTime dateTo)
        where TException : BaseDomainException, new()
    {
        if (dateFrom <  dateTo)
        {
            return;
        }

        ThrowException<TException>($"Date From: {dateFrom} must be less than Date To: {dateTo}.");
    }

    public static void AgainstOutOfRange<TException>(decimal number, decimal min, decimal max, string name = "Value")
        where TException : BaseDomainException, new()
    {
        if (min <= number && number <= max)
        {
            return;
        }

        ThrowException<TException>($"{name} must be between {min} and {max}.");
    }
    
    private static void ThrowException<TException>(string message)
        where TException : BaseDomainException, new()
    {
        var exception = new TException
        {
            Error = message
        };

        throw exception;
    }
}