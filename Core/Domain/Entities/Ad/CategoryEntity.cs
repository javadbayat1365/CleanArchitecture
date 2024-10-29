using Domain.Common;

namespace Domain.Entities.Ad;

public class CategoryEntity:BaseEntity<Guid>
{
    public string Name { get;private set; }

    private List<AdEntitiy> _ads = new();
    public IReadOnlyList<AdEntitiy> Ads => _ads.AsReadOnly();

    public CategoryEntity(){}
    public CategoryEntity(string name)
    {
       Name = name;
    }

}
