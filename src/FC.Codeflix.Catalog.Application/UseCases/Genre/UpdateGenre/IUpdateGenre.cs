using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.UpdateGenre;
public interface IUpdateGenre
    : IRequestHandler<UpdateGenreRequest, GenreModelResponse>
{ }
