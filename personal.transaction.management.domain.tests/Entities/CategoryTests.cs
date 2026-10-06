using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.enums;
using personal.transaction.management.domain.exceptions;

namespace personal.transaction.management.domain.tests.Entities;

public class CategoryTests
{
    private static Category CreateUserCategory() =>
        Category.CreateUserCategory(Guid.NewGuid(), "Food", "utensils", "#FF0000", CategoryTypeEnum.Expense);

    private static Category CreateSystemCategory() =>
        Category.CreateSystemCategory("Salary", "money", "#00FF00", CategoryTypeEnum.Income);

    [Fact]
    public void CreateUserCategory_Should_InitializeNonSystemCategory()
    {
        var userId = Guid.NewGuid();

        var category = Category.CreateUserCategory(userId, "  Food ", " utensils ", "#ff00aa", CategoryTypeEnum.Expense);

        Assert.NotEqual(Guid.Empty, category.Id);
        Assert.Equal(userId, category.UserId);
        Assert.Equal("Food", category.Name);
        Assert.Equal("utensils", category.Icon);
        Assert.Equal("#FF00AA", category.Color.Value);
        Assert.Equal(CategoryTypeEnum.Expense, category.CategoryType);
        Assert.False(category.IsSystem);
        Assert.True(category.IsActive);
    }

    [Fact]
    public void CreateSystemCategory_Should_InitializeSystemCategoryWithoutUser()
    {
        var category = CreateSystemCategory();

        Assert.Null(category.UserId);
        Assert.True(category.IsSystem);
        Assert.True(category.IsActive);
    }

    [Theory]
    [InlineData("", "icon", "Name")]
    [InlineData("  ", "icon", "Name")]
    [InlineData("Food", "", "Icon")]
    [InlineData("Food", "   ", "Icon")]
    public void CreateUserCategory_Should_Throw_When_NameOrIconIsEmpty(string name, string icon, string expectedField)
    {
        var ex = Assert.Throws<DomainValidationException>(() =>
            Category.CreateUserCategory(Guid.NewGuid(), name, icon, "#FFFFFF", CategoryTypeEnum.Expense));

        Assert.Equal(expectedField, ex.Field);
    }

    [Theory]
    [InlineData("", "icon", "Name")]
    [InlineData("Food", "", "Icon")]
    public void CreateSystemCategory_Should_Throw_When_NameOrIconIsEmpty(string name, string icon, string expectedField)
    {
        var ex = Assert.Throws<DomainValidationException>(() =>
            Category.CreateSystemCategory(name, icon, "#FFFFFF", CategoryTypeEnum.Expense));

        Assert.Equal(expectedField, ex.Field);
    }

    [Fact]
    public void CreateUserCategory_Should_Throw_When_ColorIsInvalid()
    {
        var ex = Assert.Throws<DomainValidationException>(() =>
            Category.CreateUserCategory(Guid.NewGuid(), "Food", "icon", "red", CategoryTypeEnum.Expense));

        Assert.Equal("Color", ex.Field);
    }

    [Fact]
    public void Update_Should_ChangeFields_When_CategoryIsUserOwned()
    {
        var category = CreateUserCategory();

        category.Update(" Travel ", " plane ", "#0000ff", CategoryTypeEnum.Both);

        Assert.Equal("Travel", category.Name);
        Assert.Equal("plane", category.Icon);
        Assert.Equal("#0000FF", category.Color.Value);
        Assert.Equal(CategoryTypeEnum.Both, category.CategoryType);
    }

    [Fact]
    public void Update_Should_Throw_When_CategoryIsSystem()
    {
        var category = CreateSystemCategory();

        Assert.Throws<SystemCategoryModificationException>(() =>
            category.Update("Other", "icon", "#FFFFFF", CategoryTypeEnum.Expense));

        Assert.Equal("Salary", category.Name);
    }

    [Fact]
    public void Update_Should_Throw_When_NameIsEmpty()
    {
        var category = CreateUserCategory();

        Assert.Throws<DomainValidationException>(() =>
            category.Update("", "icon", "#FFFFFF", CategoryTypeEnum.Expense));

        Assert.Equal("Food", category.Name);
    }

    [Fact]
    public void Update_Should_NotChangeFields_When_ColorIsInvalid()
    {
        var category = CreateUserCategory();

        Assert.Throws<DomainValidationException>(() =>
            category.Update("Travel", "plane", "invalid", CategoryTypeEnum.Both));

        Assert.Equal("Food", category.Name);
        Assert.Equal("#FF0000", category.Color.Value);
    }

    [Fact]
    public void Deactivate_Should_SetIsActiveToFalse_When_CategoryIsUserOwned()
    {
        var category = CreateUserCategory();

        category.Deactivate();

        Assert.False(category.IsActive);
    }

    [Fact]
    public void Deactivate_Should_Throw_When_CategoryIsSystem()
    {
        var category = CreateSystemCategory();

        Assert.Throws<SystemCategoryDeactivationException>(category.Deactivate);
        Assert.True(category.IsActive);
    }
}
