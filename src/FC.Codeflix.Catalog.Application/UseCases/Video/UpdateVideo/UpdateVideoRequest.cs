using FC.Codeflix.Catalog.Application.UseCases.Video.Common;
using FC.Codeflix.Catalog.Domain.Enums;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.UpdateVideo;

public record UpdateVideoRequest(
    Guid VideoId,
    string Title,
    string Description,
    int YearLaunched,
    bool Opened,
    bool Published,
    int Duration,
    Rating Rating,
    List<Guid>? GenresIds = null,
    List<Guid>? CategoriesIds = null,
    List<Guid>? CastMembersIds = null,
    FileInput? Banner = null,
    FileInput? Thumb = null,
    FileInput? ThumbHalf = null
) : IRequest<VideoModelResponse>;