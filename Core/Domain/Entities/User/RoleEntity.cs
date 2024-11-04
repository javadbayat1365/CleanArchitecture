using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.User;

public sealed class RoleEntity : IdentityRole<Guid>, IEntity
{
    public DateTime CreateDate { get ; set; }
    public DateTime? ModifiedDate { get ; set ; }
    public string DisplayName { get; set; }
    public RoleEntity(string displayName,string name):base(name)
    {
        DisplayName = displayName;
    }
}
