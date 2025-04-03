using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Persistence;

namespace Identity.IdentitySetup.Stores;

internal class AppRoleStore(CleanDbContext context, IdentityErrorDescriber? describer = null) 
    : RoleStore<RoleEntity, CleanDbContext, Guid>(context, describer);
