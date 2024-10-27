using Domain.Common.ValueObjects;

namespace Domain.Entities.Ad;

public class AdEntitiy
{
    private readonly List<ImageValueObject> _images = new();

    public Guid Id { get;private set; }
    public string Name { get;private set; }
    public string Description { get;private set; }
    public Guid? UserId { get;private set; }
    public IReadOnlyList<ImageValueObject> Images => _images;
    public Guid? CategoryId { get;private set; }

    private AdEntitiy() { }
    public static AdEntitiy Create(string name,string description,Guid? userId,Guid? category) {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(description);

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
            Name = name,
            Description = description,
            Id = Guid.NewGuid(),
            UserId = userId
        };
    }

    public static AdEntitiy Create(Guid? Id,string name, string description, Guid? userId, Guid? category)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(description);

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
            Name = name,
            Description = description,
            Id = Id.Value,
            UserId = userId
        };
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if(obj is not AdEntitiy adEntity) return false;
        if (ReferenceEquals(this, obj)) return true;

        return adEntity.Id.Equals(this.Id);
        
    }

    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }
}
