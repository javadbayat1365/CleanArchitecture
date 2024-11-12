using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace Application.Contracts.User;

public interface IUserManager
{
    Task<IdentityResult> CreateAsync(UserEntity user,CancellationToken cancellationToken);
    Task<UserEntity?> GetUserByUserNameAsync(string userName,CancellationToken cancellationToken);
    Task<IdentityResult> PasswordSignInAsync(UserEntity user,string givenPassword, CancellationToken cancellationToken);
}
