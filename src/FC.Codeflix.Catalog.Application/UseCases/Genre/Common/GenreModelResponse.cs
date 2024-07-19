using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
public class GenreModelResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public IReadOnlyList<GenreModelResponseCategory> Categories { get; set; }

    public GenreModelResponse(
        Guid id,
        string name,
        bool isActive,
        DateTime createdAt,
        IReadOnlyList<GenreModelResponseCategory> categories
    )
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
        Categories = categories;
    }

    public static GenreModelResponse FromGenre(
        DomainEntity.Genre genre
    ) => new(
            genre.Id,
            genre.Name,
            genre.IsActive,
            genre.CreatedAt,
            genre.Categories.Select(
                categoryId => new GenreModelResponseCategory(categoryId)
            ).ToList().AsReadOnly()
        );
}

public class GenreModelResponseCategory
{
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public GenreModelResponseCategory(Guid id, string? name = null)
        => (Id, Name) = (id, name);
}
