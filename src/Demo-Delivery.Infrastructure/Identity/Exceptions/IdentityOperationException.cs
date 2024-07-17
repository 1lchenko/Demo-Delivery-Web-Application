using Microsoft.AspNetCore.Identity;

namespace Demo_Delivery.Infrastructure.Identity.Exceptions;

public class IdentityOperationException : Exception
{
    public IdentityOperationException(string? message = null) : base(message ??
                                                                     "One or more identity errors have occurred")
    {
        Errors = new Dictionary<string, string[]>();
        if (message is not null)
        {
            Errors.Add("", new[] { message });
        }
    }

    public IdentityOperationException(IEnumerable<IdentityError> errors) : this()
    {
        var errorGroups = errors.GroupBy(x => x.Code, x => x.Description);

        foreach (var errorGroup in errorGroups)
        {
            var key = errorGroup.Key;
            var errorsGroup = errorGroup.ToArray();
            Errors.Add(key, errorsGroup);
        }
    }

    public IDictionary<string, string[]> Errors { get; }
}