namespace FC.Codeflix.Catalog.UnitTests.Domain.Entities.Category;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;
public class CategoryTestFixture
{
    public DomainEntity.Category GetValidCategory()
    => new(
        GetValidCategoryName(),
        GetValidCategoryDescription()
    );
}

[CollectionDefinition(nameof(CategoryTestFixture))]
public class CategoryTestFixtureCollection
    : ICollectionFixture<CategoryTestFixture>
{ }
