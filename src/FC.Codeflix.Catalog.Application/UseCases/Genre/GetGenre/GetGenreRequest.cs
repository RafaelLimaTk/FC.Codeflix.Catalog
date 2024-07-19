using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.GetGenre;
public class GetGenreRequest
    : IRequest<GenreModelResponse>
{
    public GetGenreRequest(Guid id)
        => Id = id;

    public Guid Id { get; set; }
}
