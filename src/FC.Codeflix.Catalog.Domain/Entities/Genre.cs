using FC.Codeflix.Catalog.Domain.SeedWorks;
using FC.Codeflix.Catalog.Domain.Validations;

namespace FC.Codeflix.Catalog.Domain.Entities;

public class Genre : AggregateRoot
{
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyList<Guid> Categories
        => _categories.AsReadOnly();

    private List<Guid> _categories;

    public Genre(string name, bool isActive = true)
    {
        Name = name;
        IsActive = isActive;
        CreatedAt = DateTime.Now;
        _categories = new List<Guid>();
        Validate();
    }

    private void Validate()
        => DomainValidation.NotNullOrEmpty(Name, nameof(Name));
}
