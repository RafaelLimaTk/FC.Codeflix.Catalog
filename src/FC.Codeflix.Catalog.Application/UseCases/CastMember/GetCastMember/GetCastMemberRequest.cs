using FC.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.CastMember.GetCastMember;
public class GetCastMemberRequest
    : IRequest<CastMemberModelResponse>
{
    public Guid Id { get; private set; }
    public GetCastMemberRequest(Guid id) => Id = id;
}
