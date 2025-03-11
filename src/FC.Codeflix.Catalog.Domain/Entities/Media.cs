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

    public void UpdateAsSentToEncode()
        => Status = MediaStatus.Processing;

    public void UpdateAsEncoded(string encodedExamplePath)
    {
        Status = MediaStatus.Completed;
        EncodedPath = encodedExamplePath;
    }

    public void UpdateAsEncodingError()
    {
        Status = MediaStatus.Error;
        EncodedPath = null;
    }
}