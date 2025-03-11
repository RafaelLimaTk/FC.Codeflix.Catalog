using FC.Codeflix.Catalog.Domain.Enums;

namespace FC.Codeflix.Catalog.Domain.Entities;

public class Media : SeedWorks.Entity
{
    public string FilePath { get; private set; }
    public string? EncodedPath { get; private set; }
    public MediaStatus Status { get; private set; }

    public Media(string filePath) : base()
    {
        FilePath = filePath;
        Status = MediaStatus.Pending;
    }
}