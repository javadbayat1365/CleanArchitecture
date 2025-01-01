using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace Application.Contracts.User;

public interface IUserManager
{
    Task<IdentityResult> CreateByPasswordAsync(UserEntity user,string password,CancellationToken cancellationToken = default);
    Task<UserEntity?> GetUserByUserNameAsync(string userName,CancellationToken cancellationToken = default);
    Task<UserEntity?> GetUserByEmailAsync(string email,CancellationToken cancellationToken = default);
    Task<UserEntity?> GetUserByIdAsync(Guid userId,CancellationToken cancellationToken=default);
    Task<IdentityResult> PasswordSignInAsync(UserEntity user,string givenPassword, CancellationToken cancellationToken = default);
}
