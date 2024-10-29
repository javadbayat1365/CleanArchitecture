using Domain.Common;
using Microsoft.AspNetCore.Identity;
using System.Transactions;

namespace Domain.Entities.User;

internal class UserEntity : IdentityUser<Guid>, IEntity
{
    public DateTime CreateDate { get ; set; }
    public DateTime? ModifiedDate { get ; set ; }

    public string FirstName { get;private set; }
    public string LastName { get; private set; }
    public string UserCode { get;private set; }

    public UserEntity(string firstName, string lastName, string userCode)
    {
        FirstName = firstName;
        LastName = lastName;
        UserCode = userCode;
    }
}
