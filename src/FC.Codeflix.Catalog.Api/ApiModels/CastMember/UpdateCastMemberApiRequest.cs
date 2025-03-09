using FC.Codeflix.Catalog.Domain.Enums;

namespace FC.Codeflix.Catalog.Api.ApiModels.CastMember;

public class UpdateCastMemberApiRequest
{
    public string Name { get; set; }
    public CastMemberType Type { get; set; }

    public UpdateCastMemberApiRequest(string name, CastMemberType type)
    {
        Name = name;
        Type = type;
    }
}