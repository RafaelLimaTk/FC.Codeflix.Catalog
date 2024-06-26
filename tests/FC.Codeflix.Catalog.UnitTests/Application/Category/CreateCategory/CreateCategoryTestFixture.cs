using FC.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;
using FC.Codeflix.Catalog.UnitTests.Application.Category.Common;

namespace FC.Codeflix.Catalog.UnitTests.Application.Category.CreateCategory;

[CollectionDefinition(nameof(CreateCategoryTestFixtureCollection))]
public class CreateCategoryTestFixtureCollection
    : ICollectionFixture<CreateCategoryTestFixture>
{ }

public class CreateCategoryTestFixture : CategoryUseCasesBaseFixture
{
    public CreateCategoryRequest GetRequest()
    => new(
        GetValidCategoryName(),
        GetValidCategoryDescription(),
        GetRandomBoolean()
    );

    public CreateCategoryRequest GetInvalidRequestShortName()
    {
        var invalidInputShortName = GetRequest();
        invalidInputShortName.Name =
            invalidInputShortName.Name.Substring(0, 2);
        return invalidInputShortName;
    }

    public CreateCategoryRequest GetInvalidRequestTooLongName()
    {
        var invalidInputTooLongName = GetRequest();
        var tooLongNameForCategory = Faker.Commerce.ProductName();
        while (tooLongNameForCategory.Length <= 255)
            tooLongNameForCategory = $"{tooLongNameForCategory} {Faker.Commerce.ProductName()}";
        invalidInputTooLongName.Name = tooLongNameForCategory;
        return invalidInputTooLongName;
    }

    public CreateCategoryRequest GetInvalidRequestCategoryNull()
    {
        var invalidInputDescriptionNull = GetRequest();
        invalidInputDescriptionNull.Description = null!;
        return invalidInputDescriptionNull;
    }

    public CreateCategoryRequest GetInvalidRequestTooLongDescription()
    {
        var invalidInputTooLongDescription = GetRequest();
        var tooLongDescriptionForCategory = Faker.Commerce.ProductDescription();
        while (tooLongDescriptionForCategory.Length <= 10_000)
            tooLongDescriptionForCategory = $"{tooLongDescriptionForCategory} {Faker.Commerce.ProductDescription()}";
        invalidInputTooLongDescription.Description = tooLongDescriptionForCategory;
        return invalidInputTooLongDescription;
    }
}
