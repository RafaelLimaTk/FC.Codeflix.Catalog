using FC.Codeflix.Catalog.Domain.Extensions;
using DomainEntities = FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.Common;

public record VideoModelResponse(
    Guid Id,
    DateTime CreatedAt,
    string Title,
    bool Published,
    string Description,
    string Rating,
    int YearLaunched,
    bool Opened,
    int Duration,
    IReadOnlyCollection<VideoModelResponseRelatedAggregate> Categories,
    IReadOnlyCollection<VideoModelResponseRelatedAggregate> Genres,
    IReadOnlyCollection<VideoModelResponseRelatedAggregate> CastMembers,

    string? ThumbFileUrl,
    string? BannerFileUrl,
    string? ThumbHalfFileUrl,
    string? VideoFileUrl,
    string? TrailerFileUrl)
{
    public static VideoModelResponse FromVideo(DomainEntities.Video video) => new(
        video.Id,
        video.CreatedAt,
        video.Title,
        video.Published,
        video.Description,
        video.Rating.ToStringSignal(),
        video.YearLaunched,
        video.Opened,
        video.Duration,
        video.Categories.Select(id => new VideoModelResponseRelatedAggregate(id)).ToList(),
        video.Genres.Select(id => new VideoModelResponseRelatedAggregate(id)).ToList(),
        video.CastMembers.Select(id => new VideoModelResponseRelatedAggregate(id)).ToList(),
        video.Thumb?.Path,
        video.Banner?.Path,
        video.ThumbHalf?.Path,
        video.Media?.FilePath,
        video.Trailer?.FilePath);

    public static VideoModelResponse FromVideo(
        DomainEntities.Video video,
        IReadOnlyList<DomainEntities.Category>? categories = null,
        IReadOnlyCollection<DomainEntities.Genre>? genres = null
    ) => new(
        video.Id,
        video.CreatedAt,
        video.Title,
        video.Published,
        video.Description,
        video.Rating.ToStringSignal(),
        video.YearLaunched,
        video.Opened,
        video.Duration,
        video.Categories.Select(id =>
            new VideoModelResponseRelatedAggregate(
                id,
                categories?.FirstOrDefault(category => category.Id == id)?.Name
            )).ToList(),
        video.Genres.Select(id =>
            new VideoModelResponseRelatedAggregate(
                id,
                genres?.FirstOrDefault(genre => genre.Id == id)?.Name
            )).ToList(),
        video.CastMembers.Select(id => new VideoModelResponseRelatedAggregate(id)).ToList(),
        video.Thumb?.Path,
        video.Banner?.Path,
        video.ThumbHalf?.Path,
        video.Media?.FilePath,
        video.Trailer?.FilePath);
}

public record VideoModelResponseRelatedAggregate(Guid Id, string? Name = null);
