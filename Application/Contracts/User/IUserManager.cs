﻿using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace Application.Contracts.User;

public interface IUserManager
{
    Task<IdentityResult> CreateByPasswordAsync(UserEntity user,string password,CancellationToken cancellationToken);
    Task<UserEntity?> GetUserByUserNameAsync(string userName,CancellationToken cancellationToken);
    Task<UserEntity?> GetUserByEmailAsync(string email,CancellationToken cancellationToken);
    Task<IdentityResult> PasswordSignInAsync(UserEntity user,string givenPassword, CancellationToken cancellationToken);
}
