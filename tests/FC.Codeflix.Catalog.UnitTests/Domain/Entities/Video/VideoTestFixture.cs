using FC.Codeflix.Catalog.UnitTests.Common;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entities.Video;

[CollectionDefinition(nameof(VideoTestFixture))]
public class VideoTestFixtureCollection : ICollectionFixture<VideoTestFixture>
{ }

public class VideoTestFixture : VideoTestFixtureBase
{ }
