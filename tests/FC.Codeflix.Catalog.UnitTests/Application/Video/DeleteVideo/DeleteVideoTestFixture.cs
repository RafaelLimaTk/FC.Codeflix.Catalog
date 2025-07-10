using FC.Codeflix.Catalog.Application.UseCases.Video.DeleteVideo;
using FC.Codeflix.Catalog.UnitTests.Common.Fixtures;

namespace FC.Codeflix.Catalog.UnitTests.Application.Video.DeleteVideo;

[CollectionDefinition(nameof(DeleteVideoTestFixture))]
public class DeleteVideoTestFixtureCollection
    : ICollectionFixture<DeleteVideoTestFixture>
{ }

public class DeleteVideoTestFixture : VideoTestFixtureBase
{
    internal DeleteVideoRequest GetValidInput(Guid? id = null)
        => new(id ?? Guid.NewGuid());
}
