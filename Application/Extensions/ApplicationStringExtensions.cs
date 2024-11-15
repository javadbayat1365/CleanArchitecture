using System.ComponentModel.DataAnnotations;

namespace Application.Extensions;

public static class ApplicationStringExtensions
{
    public static bool IsEmail(this string str)
    {
        var validator = new EmailAddressAttribute();

        return validator.IsValid(str);
    }
}
