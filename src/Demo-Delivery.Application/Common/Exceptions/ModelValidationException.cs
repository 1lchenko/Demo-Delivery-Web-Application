using FluentValidation.Results;

namespace Demo_Delivery.Application.Common.Exceptions;

public class ModelValidationException : Exception
{
    public ModelValidationException() : base("One or more validations errors have occurred")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ModelValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        var failureGroups = failures
            .GroupBy(f => f.PropertyName, e => e.ErrorMessage);

        foreach (var failureGroup in failureGroups)
        {
            var propName = failureGroup.Key;
            var propFailures = failureGroup.ToArray();
            
            Errors.Add(propName, propFailures);
        }
    }

    public Dictionary<string, string[]> Errors { get; }
}

/*public class ValidationError
{
    public string PropName { get; set; }
    public string[] ErrorMessages { get; set; }
}*/