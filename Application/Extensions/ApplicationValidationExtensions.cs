using FluentValidation.Results;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Extensions;

public static class ApplicationValidationExtensions
{
    public static List<KeyValuePair<string,string>> ConvertToKeyValuepair([NotNull]this List<ValidationFailure> failures)
    {
        return failures.Select(x => new KeyValuePair<string, string>(x.PropertyName,x.ErrorMessage)).ToList();
    }
}
