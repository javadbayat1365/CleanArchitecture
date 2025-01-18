using Domain.Common;

namespace Domain.Entities.Ad;

public sealed class LocationEntity : BaseEntity<Guid>
{
    public string Name { get;private set; }

    private List<AdEntitiy> _ads = new();

    public IReadOnlyList<AdEntitiy> Ads => _ads.AsReadOnly();

    private LocationEntity()
    {
        
    }
    public LocationEntity(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}
