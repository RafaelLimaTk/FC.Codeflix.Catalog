using FC.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using FC.Codeflix.Catalog.Domain.Enums;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.CastMember.UpdateCastMember;
public class UpdateCastMemberRequest : IRequest<CastMemberModelResponse>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public CastMemberType Type { get; set; }

    public UpdateCastMemberRequest(Guid id, string name, CastMemberType type)
    {
        this.Id = id;
        Name = name;
        Type = type;
    }
}
