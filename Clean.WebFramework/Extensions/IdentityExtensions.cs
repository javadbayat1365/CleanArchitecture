using System.Security.Claims;
using System.Security.Principal;

namespace Clean.WebFramework.Extensions;

public static class IdentityExtensions
{
    public static string? FindFirsValue(this ClaimsIdentity identity,string claimType)
    {
        return identity.FindFirst(claimType)?.Value;
    }

    public static string? FindFirsValue(this IIdentity identity, string claimType)
    {
        return identity.FindFirsValue(claimType);
    }

    public static string? GetUserId(this IIdentity identity)
    {
        return identity.FindFirsValue(ClaimTypes.NameIdentifier);
    }

    public static string? GetUserName(this IIdentity identity)
    {
        return identity?.FindFirsValue(ClaimTypes.Name);
    }

}
