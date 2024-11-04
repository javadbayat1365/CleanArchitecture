using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.User;

public sealed class UserTokenEntity : IdentityUserToken<Guid>, IEntity
{
    public DateTime CreateDate { get ; set ; }
    public DateTime? ModifiedDate { get ; set ; }

    public UserEntity  User { get; set; }
}
