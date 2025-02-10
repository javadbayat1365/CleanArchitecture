using Ardalis.GuardClauses;
using Domain.Common;
using Domain.Common.ValueObjects;
using Domain.Entities.User;
using System.Diagnostics.CodeAnalysis;

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

    #region Navigation Properties
    public CategoryEntity Category  { get; set; }
    public LocationEntity Location{ get; set; }
    public UserEntity User{ get; set; }
    #endregion

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
    public static AdEntitiy Create(string title,string description,Guid? userId,Guid categoryId,Guid? locationId) {
        //This
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(description);
        //Or This
        Guard.Against.NullOrEmpty(userId, message: "Invalid User Id");
        Guard.Against.NullOrEmpty(categoryId, message: "Invalid Category Id");
        Guard.Against.NullOrEmpty(locationId, message: "Invalid Location Id");

        var ad = new AdEntitiy()
        {
            CategoryId = categoryId,
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

    public static AdEntitiy Create(string title, string description, UserEntity user, CategoryEntity category, LocationEntity location)
    {
        //This
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(description);
        //Or This
        Guard.Against.Null(user, message: "Invalid User");
        Guard.Against.Null(category, message: "Invalid Category");
        Guard.Against.Null(location, message: "Invalid Location");

        var ad = new AdEntitiy()
        {
            Title = title,
            Description = description,
            Id = Guid.NewGuid(),
            LocationEntity = location,
            LocationId = location.Id,
            UserEntity = user,
            UserId = user.Id,
            CategoryEntity = category,
            CategoryId = category.Id,
            CurrentState = AdState.Pending
        };

        ad._logs.Add(new LogValueObject(DateTime.Now, "Ad Created!"));

        return ad;
    }

    public static AdEntitiy Create(Guid? Id,string title, string description, Guid? userId, Guid categoryId, Guid? locationId)
    {
        ArgumentNullException.ThrowIfNull(title);
        ArgumentNullException.ThrowIfNull(description);
        //OR
        Guard.Against.NullOrEmpty(userId, message: "Invalid User Id");
        Guard.Against.NullOrEmpty(Id, message: "Invalid Id");
        Guard.Against.NullOrEmpty(categoryId, message: "Invalid Category Id");
        Guard.Against.NullOrEmpty(locationId, message: "Invalid Location Id");

        
        var ad = new AdEntitiy()
        {
            CategoryId = categoryId,
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

    public void Edit(string? title,string? description,Guid? categotyId,Guid? locationId)
    {
        if (!string.IsNullOrWhiteSpace(title))
            Title = title;

        if (!string.IsNullOrWhiteSpace(description))
            Description = description;

        if (categotyId.HasValue && categotyId.Value != Guid.Empty)
            CategoryId = categotyId.Value;

        if (locationId.HasValue && locationId.Value != Guid.Empty)
            LocationId = locationId.Value;

        var domainResult = ChangeState(AdState.Pending);

        if(domainResult.IsSuccess)
        _logs.Add(new LogValueObject(DateTime.Now,"the Ad is edited!"));
        else
        _logs.Add(new LogValueObject(DateTime.Now,domainResult.Message));
    }

    public void AddImage([NotNull] ImageValueObject image)
    {
        _images.Add(image);
    }
    public void RemoveImages(string[] imageNames)
    {
        _images.RemoveAll(c => imageNames.Contains(c.FileName));
    }

}
