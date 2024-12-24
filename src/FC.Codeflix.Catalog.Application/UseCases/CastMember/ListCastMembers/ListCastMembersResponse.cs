using FC.Codeflix.Catalog.Application.Common;
using FC.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using FC.Codeflix.Catalog.Domain.SeedWorks.SearchableRepository;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.Application.UseCases.CastMember.ListCastMembers;
public class ListCastMembersResponse : PaginatedListResponse<CastMemberModelResponse>
{
    public ListCastMembersResponse(int page, int perPage, int total, IReadOnlyList<CastMemberModelResponse> items)
    : base(page, perPage, total, items)
    { }

    public static ListCastMembersResponse FromSearchOutput(
        SearchResponse<DomainEntity.CastMember> searchOutput
    ) => new(
            searchOutput.CurrentPage,
            searchOutput.PerPage,
            searchOutput.Total,
            searchOutput.Items
                .Select(castmember
                    => CastMemberModelResponse.FromCastMember(castmember))
                .ToList()
                .AsReadOnly()
        );
}
