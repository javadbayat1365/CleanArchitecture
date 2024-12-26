using Domain.Common;

namespace Domain.Entities.Ad;

public sealed class CategoryEntity:BaseEntity<Guid>
{
    public string Name { get;private set; }

    private List<AdEntitiy> _ads = new();
    public IReadOnlyList<AdEntitiy> Ads => _ads.AsReadOnly();

    private CategoryEntity(){}
    public CategoryEntity(string name)
    {
        Id = Guid.NewGuid();
       Name = name;
    }

}
