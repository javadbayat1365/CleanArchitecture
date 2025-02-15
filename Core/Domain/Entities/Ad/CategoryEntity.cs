using Ardalis.GuardClauses;
using Domain.Common;
using Domain.Common.ValueObjects;

namespace Domain.Entities.Ad;

public sealed class CategoryEntity:BaseEntity<Guid>
{
    private readonly List<LogValueObject> _logs = new();

    public string Name { get;private set; }
    private List<AdEntitiy> _ads = new();
    public IReadOnlyList<AdEntitiy> Ads => _ads.AsReadOnly();
    public IReadOnlyList<LogValueObject> logs => _logs.AsReadOnly();
    private CategoryEntity(){}
    public CategoryEntity(string name)
    {
       Id = Guid.NewGuid();
       Name = name;
       _logs.Add(new LogValueObject(DateTime.Now, string.Format("Category With {0},{1} Id,Name Has Been Created!", Id,name)));
    }

    public void EditName(string name)
    {
        Guard.Against.NullOrEmpty(Name);
        Name = name;
        _logs.Add(new LogValueObject(DateTime.Now, string.Format("Category With {0},{1} Id,Name Has Been Edited!", Id, name)));
    }

}
