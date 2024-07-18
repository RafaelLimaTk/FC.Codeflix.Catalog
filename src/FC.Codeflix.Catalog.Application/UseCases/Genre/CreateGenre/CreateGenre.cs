using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;
public class CreateGenre : ICreateGenre
{
    public Task<GenreModelResponse> Handle(CreateGenreRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
