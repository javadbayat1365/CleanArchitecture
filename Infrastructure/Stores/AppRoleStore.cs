using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence.Stores;
//test master
public class AppRoleStore(CleanDbContext context, IdentityErrorDescriber? describer=null)
    :RoleStore<RoleEntity,CleanDbContext,Guid>(context,describer);
