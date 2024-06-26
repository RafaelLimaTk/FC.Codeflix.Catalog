namespace FC.Codeflix.Catalog.UnitTests.Domain.Entities.Category;

using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
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

        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
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
        var validData = new
        {
            Name = "Category Name",
            Description = "Category description"
        };

        var datetimeBefore = DateTime.Now;
        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
        var datetimeAfter = DateTime.Now;

        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
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
        Action action =
            () => new DomainEntity.Category(name!, "Category Description");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(InstantiateErroCategoryWhenDescriptionIsNotNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErroCategoryWhenDescriptionIsNotNull()
    {
        Action action =
            () => new DomainEntity.Category("Category Name", null!);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should not be empty or null");
    }

    [Theory(DisplayName = nameof(InstantiateErroCategoryWhenNameThan3Caracters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("Ca")]
    [InlineData("C")]
    public void InstantiateErroCategoryWhenNameThan3Caracters(string name)
    {
        Action action =
            () => new DomainEntity.Category(name, "Category Description");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at leats 3 caracters long");
    }

    [Fact(DisplayName = nameof(InstantiateErroCategoryWhenNameThan255Caracters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErroCategoryWhenNameThan255Caracters()
    {
        var name = new string('a', 256);

        Action action =
            () => new DomainEntity.Category(name, "Category Description");

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 caracters long");
    }

    [Fact(DisplayName = nameof(InstantiateErroCategoryWhenDescriptionThan10_000Caracters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErroCategoryWhenDescriptionThan10_000Caracters()
    {
        var description = new string('a', 10_001);

        Action action =
            () => new DomainEntity.Category("Category Name", description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal 10.000 caracters long");
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

        category.IsActive.Should().BeTrue();
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

        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(UpdateCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateCategory()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var newName = "New Category Name";
        var newDescription = "New Category description";

        category.Update(newName, newDescription);

        category.Name.Should().Be(newName);
        category.Description.Should().Be(newDescription);
    }

    [Fact(DisplayName = nameof(UpdateOnlyNameCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyNameCategory()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var newName = "New Category Name";

        category.Update(newName);

        category.Name.Should().Be(newName);
        category.Description.Should().Be(validData.Description);
    }

    [Fact(DisplayName = nameof(UpdateErroNameIsEmptyCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErroNameIsEmptyCategory()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description);

        Action action =
            () => category.Update("");

        //var exception = Assert.Throws<EntityValidationException>(action);
        //Assert.Equal("Name should not be empty or null", exception.Message);

        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateErroNameThan3CaractersCategory))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("Ca")]
    [InlineData("C")]
    public void UpdateErroNameThan3CaractersCategory(string name)
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description);

        Action action =
            () => category.Update(name);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be at leats 3 caracters long");
    }

    [Fact(DisplayName = nameof(UpdateErroNameThan255CaractersCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErroNameThan255CaractersCategory()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var name = new string('a', 256);

        Action action =
            () => category.Update(name);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should be less or equal 255 caracters long");
    }

    [Fact(DisplayName = nameof(UpdateErroDescriptionThan10_000CaractersCategory))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErroDescriptionThan10_000CaractersCategory()
    {
        var validData = new
        {
            Name = "Category Name",
            Description = "Category description"
        };

        var category = new DomainEntity.Category(validData.Name, validData.Description);
        var description = new string('a', 10_001);

        Action action =
            () => category.Update("Category Name", description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Description should be less or equal 10.000 caracters long");
    }
}
