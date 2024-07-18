using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;

public class CreateGenreRequest : IRequest<GenreModelResponse>
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public List<Guid>? CategoriesId { get; set; }

    public CreateGenreRequest(
        string name,
        bool isActive,
        List<Guid>? categoriesId = null
    )
    {
        Name = name;
        IsActive = isActive;
        CategoriesId = categoriesId;
    }
}
