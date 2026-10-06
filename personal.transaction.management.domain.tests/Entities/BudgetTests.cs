using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.exceptions;

namespace personal.transaction.management.domain.tests.Entities;

public class BudgetTests
{
    private static readonly DateOnly Start = new(2026, 10, 1);
    private static readonly DateOnly End = new(2026, 10, 31);

    private static Budget CreateBudget() =>
        Budget.Create(Guid.NewGuid(), Guid.NewGuid(), Start, End, "October food", 500m);

    [Fact]
    public void Create_Should_InitializeBudget_When_DataIsValid()
    {
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var budget = Budget.Create(userId, categoryId, Start, End, "October food", 500m);

        Assert.NotEqual(Guid.Empty, budget.Id);
        Assert.Equal(userId, budget.UserId);
        Assert.Equal(categoryId, budget.CategoryId);
        Assert.Equal("October food", budget.Name);
        Assert.Equal(Start, budget.StartDate);
        Assert.Equal(End, budget.EndDate);
        Assert.Equal(500m, budget.Amount);
        Assert.True(budget.IsActive);
    }

    [Fact]
    public void Create_Should_AllowZeroAmount()
    {
        var budget = Budget.Create(Guid.NewGuid(), Guid.NewGuid(), Start, End, "Zero", 0m);

        Assert.Equal(0m, budget.Amount);
    }

    [Fact]
    public void Create_Should_Throw_When_AmountIsNegative()
    {
        Assert.Throws<BudgetAmountNegativeException>(() =>
            Budget.Create(Guid.NewGuid(), Guid.NewGuid(), Start, End, "Food", -1m));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Create_Should_Throw_When_NameIsEmpty(string? name)
    {
        Assert.Throws<BudgetNameEmptyException>(() =>
            Budget.Create(Guid.NewGuid(), Guid.NewGuid(), Start, End, name!, 100m));
    }

    [Fact]
    public void Update_Should_ChangeFields_When_DataIsValid()
    {
        var budget = CreateBudget();
        var newStart = new DateOnly(2026, 11, 1);
        var newEnd = new DateOnly(2026, 11, 30);

        budget.Update("November food", newStart, newEnd, 650m);

        Assert.Equal("November food", budget.Name);
        Assert.Equal(newStart, budget.StartDate);
        Assert.Equal(newEnd, budget.EndDate);
        Assert.Equal(650m, budget.Amount);
    }

    [Fact]
    public void Update_Should_Throw_When_AmountIsNegative()
    {
        var budget = CreateBudget();

        Assert.Throws<BudgetAmountNegativeException>(() => budget.Update("Food", Start, End, -10m));
        Assert.Equal(500m, budget.Amount);
    }

    [Fact]
    public void Update_Should_Throw_When_NameIsEmpty()
    {
        var budget = CreateBudget();

        Assert.Throws<BudgetNameEmptyException>(() => budget.Update(" ", Start, End, 100m));
        Assert.Equal("October food", budget.Name);
    }

    [Fact]
    public void Deactivate_And_Activate_Should_ToggleIsActive()
    {
        var budget = CreateBudget();

        budget.Deactivate();
        Assert.False(budget.IsActive);

        budget.Activate();
        Assert.True(budget.IsActive);
    }
}
