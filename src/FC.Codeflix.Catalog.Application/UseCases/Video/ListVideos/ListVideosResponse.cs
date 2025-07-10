using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.Video.Common;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.ListVideos;
public class ListVideosResponse : PaginatedListResponse<VideoModelResponse>
{
    public ListVideosResponse(
        int page,
        int perPage,
        int total,
        IReadOnlyList<VideoModelResponse> items)
        : base(page, perPage, total, items)
    { }
}
