namespace Domain.Entities.Ad;

public sealed class LocationEntity
{
    public string Name { get;private set; }

    private List<AdEntitiy> _ads = new();

    public IReadOnlyList<AdEntitiy> ads => _ads.AsReadOnly();

    private LocationEntity()
    {
        
    }
    public LocationEntity(string name)
    {
        Name = name;
    }
}
