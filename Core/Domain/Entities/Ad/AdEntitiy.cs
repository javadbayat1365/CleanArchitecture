using Ardalis.GuardClauses;
using Domain.Common;
using Domain.Common.ValueObjects;
using System.Runtime.CompilerServices;

namespace Domain.Entities.Ad;

public sealed class AdEntitiy : BaseEntity<Guid>
{
    private readonly List<ImageValueObject> _images = new();
    private readonly List<LogValueObject> _logs = new();

    public string Title { get;private set; }
    public string Description { get;private set; }
    public Guid? UserId { get;private set; }
    public IReadOnlyList<ImageValueObject> Images => _images.AsReadOnly();
    public IReadOnlyList<LogValueObject> Logs => _logs.AsReadOnly();
    public Guid CategoryId { get;private set; }
    public Guid LocationId { get;private set; }
    public AdState CurrentState { get;private set; } 

    public enum AdState
    {
        Pending,
        Rejected,
        Approved,
        Deleted,
        Expired
    } 

    public DomainResult ChangeState(AdState adState)
    {
        if (CurrentState is AdState.Approved && adState is AdState.Rejected or AdState.Pending)
        {
            return new DomainResult(false,"This ad is already approved!");
        }

        CurrentState = adState;
        _logs.Add(new LogValueObject(DateTime.Now, "Ad State Changed!"));
        return DomainResult.None;
    }

    private AdEntitiy() { }
    public static AdEntitiy Create(string title,string description,Guid? userId,Guid category,Guid? locationId) {
        //This
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(description);
        //Or
        Guard.Against.NullOrEmpty(userId, message: "Invalid User Id");
        Guard.Against.NullOrEmpty(category, message: "Invalid Category Id");
        Guard.Against.NullOrEmpty(locationId, message: "Invalid Location Id");

        var ad = new AdEntitiy()
        {
            CategoryId = category,
            Title = title,
            Description = description,
            Id = Guid.NewGuid(),
            UserId = userId,
            CurrentState = AdState.Pending,
            LocationId = locationId.Value
        };

        ad._logs.Add(new LogValueObject(DateTime.Now,"Ad Created!"));

        return ad;
    }

    public static AdEntitiy Create(Guid? Id,string title, string description, Guid? userId, Guid category, Guid? locationId)
    {
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(description);

        Guard.Against.NullOrEmpty(userId, message: "Invalid User Id");
        Guard.Against.NullOrEmpty(Id, message: "Invalid Id");
        Guard.Against.NullOrEmpty(category, message: "Invalid Category Id");
        Guard.Against.NullOrEmpty(locationId, message: "Invalid Location Id");

        
        var ad = new AdEntitiy()
        {
            CategoryId = category,
            Title = title,
            Description = description,
            Id = Id.Value,
            UserId = userId,
            CurrentState = AdState.Pending,
            LocationId = locationId.Value
        };

        ad._logs.Add(new LogValueObject(DateTime.Now, "Ad Created!"));

        return ad;
    }

    public void Edit(string title,string description,Guid categotyId,Guid? locationId)
    {
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(description);

        Guard.Against.NullOrEmpty(locationId, message: "Invalid Location Id");

        CategoryId = categotyId;
        Title = title;
        Description = description; 
        LocationId = locationId.Value;

        var domainResult = ChangeState(AdState.Pending);
        if(domainResult.IsSuccess)
        _logs.Add(new LogValueObject(DateTime.Now,"the Ad is edited!"));
        else
        _logs.Add(new LogValueObject(DateTime.Now,domainResult.Message));
    }

}
