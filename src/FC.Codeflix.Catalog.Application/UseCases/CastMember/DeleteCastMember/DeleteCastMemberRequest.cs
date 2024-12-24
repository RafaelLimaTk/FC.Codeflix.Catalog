using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.CastMember.DeleteCastMember;
public class DeleteCastMemberRequest
    : IRequest
{
    public Guid Id { get; private set; }
    public DeleteCastMemberRequest(Guid id) => Id = id;
}
