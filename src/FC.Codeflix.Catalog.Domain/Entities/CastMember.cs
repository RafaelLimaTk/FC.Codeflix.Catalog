﻿using FC.Codeflix.Catalog.Domain.Enums;
using FC.Codeflix.Catalog.Domain.SeedWorks;

namespace FC.Codeflix.Catalog.Domain.Entities;
public class CastMember : AggregateRoot
{
    public string Name { get; private set; }
    public CastMemberType Type { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public CastMember(string name, CastMemberType type)
        : base()
    {
        Name = name;
        Type = type;
        CreatedAt = DateTime.Now;
    }
}
