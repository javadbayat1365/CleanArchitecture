using Application.Contracts.User;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace Identity.Services.Implementations;

internal class UserManagerImplementation(UserManager<UserEntity> userManager,SignInManager<UserEntity> signInManager) : IUserManager
{
    public async Task<IdentityResult> CreateByPasswordAsync(UserEntity user, string password, CancellationToken cancellationToken = default)
    {
        return await userManager.CreateAsync(user, password);  
    }

    public async Task<UserEntity?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await userManager.FindByEmailAsync(email);
    }

    public async Task<UserEntity?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<UserEntity?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await userManager.FindByNameAsync(userName);
    }

    public async Task<IdentityResult> PasswordSignInAsync(UserEntity user, string givenPassword, CancellationToken cancellationToken = default)
    {
        var checkPassword = await signInManager.CheckPasswordSignInAsync(user,givenPassword,true);
        if (checkPassword.Succeeded)
        {
            return IdentityResult.Success;
        }
        return IdentityResult.Failed(new IdentityError() { Code="InvalidPassword", Description="Password is not correct!" });
    }
}
