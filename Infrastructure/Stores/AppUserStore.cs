using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence.Stores;

public class AppUserStore(CleanDbContext context,IdentityErrorDescriber? describer = null)
    : UserStore<UserEntity, RoleEntity, CleanDbContext, Guid, UserClaimEntity, UserRoleEntity, UserLoginEntity, UserTokenEntity, RoleClaimEntity>(context, describer);

