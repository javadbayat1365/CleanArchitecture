using Domain.Common;
using Domain.Common.ValueObjects;

namespace Domain.Entities.Ad;

public class AdEntitiy : BaseEntity<Guid>
{
    private readonly List<ImageValueObject> _images = new();

    public Guid Id { get;private set; }
    public string Title { get;private set; }
    public string Description { get;private set; }
    public Guid? UserId { get;private set; }
    public IReadOnlyList<ImageValueObject> Images => _images;
    public Guid? CategoryId { get;private set; }
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

        this.CurrentState = adState;

        return DomainResult.None;
    }

    //factory method
    private AdEntitiy() { }
    public static AdEntitiy Create(string title,string description,Guid? userId,Guid? category,Guid? locationId) {
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(description);


        //Ardelin.Guard Library
        //Guard.Against.NullOrEmpty(userId,message:"Invalid User Id");
        //Guard.Against.NullOrEmpty(category, message: "Invalid Category Id");
        //Guard.Against.NullOrEmpty(locationId, message: "Invalid Location Id");

        if (locationId == null || locationId == Guid.Empty)
        {
            throw new InvalidOperationException("Location id must have a value!");
        }

        if (userId == null || userId == Guid.Empty)
        {
            throw new InvalidOperationException("User id must have a value!");
        }
        if (category == null || category == Guid.Empty)
        {
            throw new InvalidOperationException("Category must have a value!");
        }

        return new AdEntitiy()
        {
            CategoryId = category,
            Title = title,
            Description = description,
            Id = Guid.NewGuid(),
            UserId = userId,
            CurrentState = AdState.Pending,
            LocationId = locationId.Value
        };
    }

    public static AdEntitiy Create(Guid? Id,string title, string description, Guid? userId, Guid? category, Guid? locationId)
    {
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(description);

        //Ardelin.Guard Library
        //Guard.Against.NullOrEmpty(userId,message:"Invalid User Id");
        //Guard.Against.NullOrEmpty(Id,message:"Invalid Id");
        //Guard.Against.NullOrEmpty(category, message: "Invalid Category Id");
        //Guard.Against.NullOrEmpty(locationId, message: "Invalid Location Id");

        if (locationId == null || locationId == Guid.Empty)
        {
            throw new InvalidOperationException("Location id must have a value!");
        }
        if (userId == null || userId == Guid.Empty)
        {
            throw new InvalidOperationException("User id must have a value!");
        }
        if (category == null || category == Guid.Empty)
        {
            throw new InvalidOperationException("Category must have a value!");
        }
        if (Id == null || Id == Guid.Empty)
        {
            throw new InvalidOperationException("Id must have a value!");
        }
        return new AdEntitiy()
        {
            CategoryId = category,
            Title = title,
            Description = description,
            Id = Id.Value,
            UserId = userId,
            CurrentState = AdState.Pending,
            LocationId = locationId.Value
        };
    }

}
