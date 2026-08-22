using personal.transaction.management.domain.abstractions;
using personal.transaction.management.domain.exceptions;
namespace personal.transaction.management.domain.Entities;

public class Budget : BaseAuditable
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid CategoryId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public decimal Amount { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Budget() { }

    private Budget(
        Guid userId,
        Guid categoryId,
        string name,
        DateOnly startDate,
        DateOnly endDate,
        decimal budgetAmount)
    {
        if (budgetAmount < 0) throw new BudgetAmountNegativeException(budgetAmount);
        if (string.IsNullOrWhiteSpace(name)) throw new BudgetNameEmptyException();

        Id = Guid.NewGuid();
        UserId = userId;
        CategoryId = categoryId;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        Amount = budgetAmount;
        IsActive = true;
    }

    public static Budget Create(
        Guid userId,
        Guid categoryId,
        DateOnly startDate,
        DateOnly endDate,
        string name,
        decimal budgetAmount)
     => new(userId, categoryId, name, startDate, endDate, budgetAmount);

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
    public void Update(
        string name,
        DateOnly startDate,
        DateOnly endDate,
        decimal budgetAmount)
    {
        if (budgetAmount < 0) throw new BudgetAmountNegativeException(budgetAmount);
        if (string.IsNullOrWhiteSpace(name)) throw new BudgetNameEmptyException();

        Name = name;
        StartDate = startDate;
        EndDate = endDate;
        Amount = budgetAmount;
    }
}
