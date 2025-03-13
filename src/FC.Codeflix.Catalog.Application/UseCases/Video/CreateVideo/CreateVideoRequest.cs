using FC.Codeflix.Catalog.Application.UseCases.Video.Common;
using FC.Codeflix.Catalog.Domain.Enums;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.CreateVideo;

public record CreateVideoRequest(
    string? Title,
    string? Description,
    int YearLaunched,
    bool Opened,
    bool Published,
    int Duration,
    Rating Rating,
    IReadOnlyCollection<Guid>? CategoriesIds = null,
    IReadOnlyCollection<Guid>? GenresIds = null,
    IReadOnlyCollection<Guid>? CastMembersIds = null,
    FileInput? Thumb = null,
    FileInput? Banner = null,
    FileInput? ThumbHalf = null,
    FileInput? Media = null,
    FileInput? Trailer = null
) : IRequest<VideoModelResponse>;