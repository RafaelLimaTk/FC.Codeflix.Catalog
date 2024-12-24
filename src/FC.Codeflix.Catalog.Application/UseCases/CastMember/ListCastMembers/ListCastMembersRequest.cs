using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Domain.SeedWorks.SearchableRepository;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.CastMember.ListCastMembers;
public class ListCastMembersRequest
    : PaginatedListRequest, IRequest<ListCastMembersResponse>
{
    public ListCastMembersRequest(int page, int perPage, string search, string sort, SearchOrder dir)
    : base(page, perPage, search, sort, dir)
    { }

    public ListCastMembersRequest()
        : base(1, 15, "", "", SearchOrder.Asc)
    { }
}
