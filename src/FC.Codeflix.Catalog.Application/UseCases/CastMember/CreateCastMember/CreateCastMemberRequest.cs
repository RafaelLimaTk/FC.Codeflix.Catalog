using FC.Codeflix.Catalog.Application.UseCases.CastMember.Common;
using FC.Codeflix.Catalog.Domain.Enums;
using MediatR;

namespace FC.Codeflix.Catalog.Application.UseCases.CastMember.CreateCastMember;
public class CreateCastMemberRequest
    : IRequest<CastMemberModelResponse>
{
    public string Name { get; private set; }
    public CastMemberType Type { get; private set; }

    public CreateCastMemberRequest(string name, CastMemberType type)
    {
        Name = name;
        Type = type;
    }
}
