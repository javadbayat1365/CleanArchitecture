using Microsoft.AspNetCore.Identity;

namespace Application.Extensions;

public static class ApplicationIdentityExtensions
{
    public static List<KeyValuePair<string,string>> ConvertToKeyValuePair(this IEnumerable<IdentityError> errors)
    {
        return errors.Select(x => new KeyValuePair<string,string>(x.Code,x.Description)).ToList();
    }
}
