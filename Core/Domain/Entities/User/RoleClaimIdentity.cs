using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.User;

internal class RoleClaimIdentity : IdentityRoleClaim<Guid>, IEntity
{
    public DateTime CreateDate { get; set; }
    public DateTime? ModifiedDate { get ; set; }
}
