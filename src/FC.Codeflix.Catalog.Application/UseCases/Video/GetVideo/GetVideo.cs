using FC.Codeflix.Catalog.Application.UseCases.Video.Common;
using FC.Codeflix.Catalog.Domain.Interfaces;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.GetVideo;

public class GetVideo : IGetVideo
{
    private readonly IVideoRepository _repository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IGenreRepository _genreRepository;

    public GetVideo(IVideoRepository repository,
        ICategoryRepository categoryRepository,
        IGenreRepository genreRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _genreRepository = genreRepository;
    }

    public async Task<VideoModelResponse> Handle(
        GetVideoRequest request,
        CancellationToken cancellationToken)
    {
        var video = await _repository.Get(request.VideoId, cancellationToken);
        IReadOnlyList<DomainEntity.Category>? categories = null;
        var relatedCategoriesIds = video.Categories?.Distinct().ToList();
        if (relatedCategoriesIds is not null && relatedCategoriesIds.Any())
            categories = await _categoryRepository.GetListByIds(relatedCategoriesIds, cancellationToken);

        IReadOnlyList<DomainEntity.Genre>? genres = null;
        var relatedGenresIds = video.Genres?.Distinct().ToList();
        if (relatedGenresIds is not null && relatedGenresIds.Count > 0)
            genres = await _genreRepository.GetListByIds(relatedGenresIds, cancellationToken);

        return VideoModelResponse.FromVideo(video, categories, genres);
    }
}