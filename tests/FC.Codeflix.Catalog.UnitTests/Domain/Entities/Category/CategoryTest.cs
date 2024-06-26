namespace FC.Codeflix.Catalog.UnitTests.Domain.Entities.Category;

using FC.Codeflix.Catalog.Domain.Exceptions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entities;

public class CategoryTest
{
    [Fact(DisplayName = nameof(InstantiateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateCategory()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category description"
        };

        var datetimeBefore = DateTime.Now;  
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var datetimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.True(category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateCategoryWithIsActiveStatus))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateCategoryWithIsActiveStatus(bool isActive)
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category description"
        };

        var datetimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
        var datetimeAfter = DateTime.Now;

        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateErroCategoryWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void InstantiateErroCategoryWhenNameIsEmpty(string name)
    {
        Action action =
            () => new DomainEntity.Category(name!, "Category Description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErroCategoryWhenDescriptionIsNotNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErroCategoryWhenDescriptionIsNotNull()
    {
        Action action =
            () => new DomainEntity.Category("Category Name", null!);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(InstantiateErroCategoryWhenNameThan3Caracters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("Ca")]
    [InlineData("C")]
    public void InstantiateErroCategoryWhenNameThan3Caracters(string name)
    {
        Action action =
            () => new DomainEntity.Category(name, "Category Description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be at leats 3 caracters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErroCategoryWhenNameThan255Caracters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErroCategoryWhenNameThan255Caracters()
    {
        var name = new string('a', 256);

        Action action =
            () => new DomainEntity.Category(name, "Category Description");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should be less or equal 255 caracters long", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErroCategoryWhenDescriptionThan10_000Caracters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErroCategoryWhenDescriptionThan10_000Caracters()
    {
        var description = new string('a', 10_001);

        Action action =
            () => new DomainEntity.Category("Category Name", description);

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should be less or equal 10.000 caracters long", exception.Message);
    }

    [Fact(DisplayName = nameof(ActivateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void ActivateCategory()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description, false);
        category.Activate();

        Assert.True(category.IsActive);
    }

    [Fact(DisplayName = nameof(DeactivateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void DeactivateCategory()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description);
        category.Deactivate();

        Assert.False(category.IsActive);
    }
}
