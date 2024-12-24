using FC.Codeflix.Catalog.Domain.Interfaces;

namespace FC.Codeflix.Catalog.Application.UseCases.CastMember.ListCastMembers;
public class ListCastMembers : IListCastMembers
{
    private readonly ICastMemberRepository _repository;

    public ListCastMembers(ICastMemberRepository repository)
        => _repository = repository;

    public async Task<ListCastMembersResponse> Handle(
        ListCastMembersRequest request,
        CancellationToken cancellationToken
    )
    {
        var searchOutput = await _repository.Search(request.ToSearchRequest(), cancellationToken);
        return ListCastMembersResponse.FromSearchOutput(searchOutput);
    }
}
