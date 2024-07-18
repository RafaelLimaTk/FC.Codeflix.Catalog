using FC.Codeflix.Catalog.Application.Exceptions;
using FC.Codeflix.Catalog.Application.Interfaces;
using FC.Codeflix.Catalog.Application.UseCases.Genre.Common;
using FC.Codeflix.Catalog.Domain.Interfaces;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.Application.UseCases.Genre.CreateGenre;
public class CreateGenre : ICreateGenre
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGenre(
        IGenreRepository genreRepository,
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository
    )
    {
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
    }


    public async Task<GenreModelResponse> Handle(
        CreateGenreRequest request, CancellationToken cancellationToken
    )
    {
        var genre = new DomainEntity.Genre(
            request.Name,
            request.IsActive
        );

        if ((request.CategoriesId?.Count ?? 0) > 0)
        {
            await ValidateCategoriesIds(request, cancellationToken);
            request.CategoriesId?.ForEach(genre.AddCategory);
        }

        await _genreRepository.Insert(genre, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);
        return GenreModelResponse.FromGenre(genre);
    }

    private async Task ValidateCategoriesIds(
        CreateGenreRequest request,
        CancellationToken cancellationToken
    )
    {
        var IdsInPersistence = await _categoryRepository
            .GetIdsListByIds(
                request.CategoriesId!,
                cancellationToken
            );
        if (IdsInPersistence.Count < request.CategoriesId!.Count)
        {
            var notFoundIds = request.CategoriesId
                .FindAll(x => !IdsInPersistence.Contains(x));
            var notFoundIdsAsString = String.Join(", ", notFoundIds);
            throw new RelatedAggregateException(
                $"Related category id (or ids) not found: {notFoundIdsAsString}"
            );
        }
    }
}
