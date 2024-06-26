namespace FC.Codeflix.Catalog.UnitTests.Domain.Entities.Category;

using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _fixture;

    public CategoryTest(CategoryTestFixture fixture) => _fixture = fixture;

    [Fact(DisplayName = nameof(InstantiateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateCategory()
    {
        var validCategory = _fixture.GetValidCategory();

        var datetimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
        var datetimeAfter = DateTime.Now;

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        (category.IsActive).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateCategoryWithIsActiveStatus))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateCategoryWithIsActiveStatus(bool isActive)
    {
        var validCategory = _fixture.GetValidCategory();

        var datetimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
        var datetimeAfter = DateTime.Now;

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        (category.IsActive).Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantiateErroCategoryWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void InstantiateErroCategoryWhenNameIsEmpty(string name)
    {
        Action action = () => new DomainEntity.Category(name!, "Category Description");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(InstantiateErroCategoryWhenDescriptionIsNotNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErroCategoryWhenDescriptionIsNotNull()
    {
        Action action = () => new DomainEntity.Category("Category Name", null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should not be empty or null");
    }

    [Theory(DisplayName = nameof(InstantiateErroCategoryWhenNameThan3Caracters))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(GetNamesWithLessThan3Characters), parameters: 10)]
    public void InstantiateErroCategoryWhenNameThan3Caracters(string name)
    {
        Action action = () => new DomainEntity.Category(name, "Category Description");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at leats 3 caracters long");
    }

    public static IEnumerable<object[]> GetNamesWithLessThan3Characters(int numberOfTests = 6)
    {
        var fixture = new CategoryTestFixture();
        for (int i = 0; i < numberOfTests; i++)
        {
            var isOdd = i % 2 == 1;
            yield return new object[] {
                fixture.GetValidCategoryName()[..(isOdd ? 1 : 2)]
            };
        }
    }

    [Fact(DisplayName = nameof(InstantiateErroCategoryWhenNameThan255Caracters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErroCategoryWhenNameThan255Caracters()
    {
        var name = new string('a', 256);

        Action action = () => new DomainEntity.Category(name, "Category Description");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 caracters long");
    }

    [Fact(DisplayName = nameof(InstantiateErroCategoryWhenDescriptionThan10_000Caracters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErroCategoryWhenDescriptionThan10_000Caracters()
    {
        var description = new string('a', 10_001);

        Action action = () => new DomainEntity.Category("Category Name", description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal 10000 caracters long");
    }

    [Fact(DisplayName = nameof(ActivateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void ActivateCategory()
    {
        var category = _fixture.GetValidCategory();
        category.Deactivate();
        category.Activate();

        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(DeactivateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void DeactivateCategory()
    {
        var category = _fixture.GetValidCategory();
        category.Deactivate();

        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateCategory()
    {
        var category = _fixture.GetValidCategory();
        var newName = _fixture.GetValidCategoryName();
        var newDescription = _fixture.GetValidCategoryDescription();

        category.Update(newName, newDescription);

        category.Name.Should().Be(newName);
        category.Description.Should().Be(newDescription);
    }

    [Fact(DisplayName = nameof(UpdateOnlyNameCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyNameCategory()
    {
        var category = _fixture.GetValidCategory();
        var newName = _fixture.GetValidCategoryName();

        category.Update(newName);

        category.Name.Should().Be(newName);
        category.Description.Should().Be(category.Description);
    }

    [Fact(DisplayName = nameof(UpdateErroNameIsEmptyCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErroNameIsEmptyCategory()
    {
        var category = _fixture.GetValidCategory();

        Action action = () => category.Update("");

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateErroNameThan3CaractersCategory))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("Ca")]
    [InlineData("C")]
    public void UpdateErroNameThan3CaractersCategory(string name)
    {
        var category = _fixture.GetValidCategory();

        Action action = () => category.Update(name);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at leats 3 caracters long");
    }

    [Fact(DisplayName = nameof(UpdateErroNameThan255CaractersCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErroNameThan255CaractersCategory()
    {
        var category = _fixture.GetValidCategory();
        var name = new string('a', 256);

        Action action = () => category.Update(name);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 caracters long");
    }

    [Fact(DisplayName = nameof(UpdateErroDescriptionThan10_000CaractersCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErroDescriptionThan10_000CaractersCategory()
    {
        var category = _fixture.GetValidCategory();
        var description = new string('a', 10_001);

        Action action = () => category.Update(category.Name, description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal 10000 caracters long");
    }
}
