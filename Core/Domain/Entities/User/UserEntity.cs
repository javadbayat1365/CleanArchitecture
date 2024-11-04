using Domain.Common;
using Domain.Entities.Ad;
using Microsoft.AspNetCore.Identity;
using System.Transactions;

namespace Domain.Entities.User;

public sealed class UserEntity : IdentityUser<Guid>, IEntity
{
    public DateTime CreateDate { get ; set; }
    public DateTime? ModifiedDate { get ; set ; }

    public string FirstName { get;private set; }
    public string LastName { get; private set; }
    public string UserCode { get;private set; }

    private List<AdEntitiy> _ads = new();

    public IReadOnlyList<AdEntitiy> ads => _ads.AsReadOnly();


    public UserEntity(string firstName, string lastName,string userName,string email):base(userName)
    {
        FirstName = firstName;
        LastName = lastName;
        UserCode = Guid.NewGuid().ToString("N")[0..7];
        Email = email;
    }
}
