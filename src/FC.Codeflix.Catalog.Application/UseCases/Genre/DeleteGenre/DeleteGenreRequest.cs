using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.DeleteGenre;
public class DeleteGenreRequest
    : IRequest
{
    public DeleteGenreRequest(Guid id) => Id = id;

    public Guid Id { get; set; }
}
