using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Domain.SeedWorks.SearchableRepository;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.Video.ListVideos;
public class ListVideosRequest : PaginatedListRequest, IRequest<ListVideosResponse>
{
    public ListVideosRequest(
        int page = 1,
        int perPage = 15,
        string search = "",
        string sort = "",
        SearchOrder dir = SearchOrder.Asc)
        : base(page, perPage, search, sort, dir)
    { }
}
