using FC.Codeflix.Catalog.Domain.Enums;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;

namespace FC.Codeflix.Catalog.Application.UseCases.CastMember.Common;
public class CastMemberModelResponse
{
    public CastMemberModelResponse(
        Guid id,
        string name,
        CastMemberType type,
        DateTime createdAt
    )
    {
        Id = id;
        Name = name;
        Type = type;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public CastMemberType Type { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static CastMemberModelResponse FromCastMember(DomainEntity.CastMember castMember)
        => new(
            castMember.Id,
            castMember.Name,
            castMember.Type,
            castMember.CreatedAt
        );

    public override string ToString()
    {
        return $"[Id] = {Id}, [Name] = {Name}, [CreatedAt] = {CreatedAt:HHmmfffffff}";
    }
}
