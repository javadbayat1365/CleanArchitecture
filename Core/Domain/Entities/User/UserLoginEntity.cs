﻿using Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.User;

public sealed class UserLoginEntity : IdentityUserLogin<Guid>, IEntity
{
    public DateTime CreateDate { get; set ; }
    public DateTime? ModifiedDate { get ; set ; }

    public UserEntity User { get; set; }
}
