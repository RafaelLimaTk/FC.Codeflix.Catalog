using FC.Codeflix.Catalog.Application.Common;

namespace FC.Codeflix.Catalog.Api.ApiModels.Response;

public class ApiResponseList<TItemData>
    : ApiResponse<IReadOnlyList<TItemData>>
{
    public ApiResponseListMeta Meta { get; private set; }

    public ApiResponseList(
        IReadOnlyList<TItemData> data,
        int currentPage,
        int perPage,
        int total
    )
        : base(data)
    {
        Meta = new(currentPage, perPage, total);
    }

    public ApiResponseList(
        PaginatedListResponse<TItemData> paginatedListResponse
    )
        : base(paginatedListResponse.Items)
    {
        Meta = new(
            paginatedListResponse.Page,
            paginatedListResponse.PerPage,
            paginatedListResponse.Total
        );
    }
}
