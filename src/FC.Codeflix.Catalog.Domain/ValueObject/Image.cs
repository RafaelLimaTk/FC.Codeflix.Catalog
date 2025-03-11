
namespace FC.Codeflix.Catalog.Domain.ValueObject;

public class Image : SeedWorks.ValueObject
{
    public string Path { get; }

    public Image(string path) => Path = path;

    public override bool Equals(SeedWorks.ValueObject? other) =>
        other is Image image &&
        Path == image.Path;

    protected override int GetCustomHashCode()
        => HashCode.Combine(Path);
}
