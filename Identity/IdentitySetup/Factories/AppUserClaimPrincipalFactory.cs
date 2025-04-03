using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Identity.IdentitySetup.Factories;

internal class AppUserClaimPrincipalFactory(
        UserManager<UserEntity> userManager,
        RoleManager<RoleEntity> roleManager,
        IOptions<IdentityOptions> options) 
        : UserClaimsPrincipalFactory<UserEntity, RoleEntity>(userManager, roleManager, options)
{
   
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(UserEntity user)
    {

        var claimIdentity = await base.GenerateClaimsAsync(user);
        var userRoles = await userManager.GetRolesAsync(user);

        foreach (var role in userRoles)
        {
            claimIdentity.AddClaim(new Claim(ClaimTypes.Role,role));
        }

        claimIdentity.AddClaim(new Claim(ClaimTypes.UserData,user.UserCode));

        return claimIdentity;
    }
}
