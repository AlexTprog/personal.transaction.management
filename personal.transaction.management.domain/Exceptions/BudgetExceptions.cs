namespace personal.transaction.management.domain.exceptions;

public sealed class BudgetAlreadyExistsException : DomainException
{
    public BudgetAlreadyExistsException(int year, int month, Guid categoryId)
        : base($"A budget for the year {year}, month {month}, and category {categoryId} already exists.") { }
}

public sealed class BudgetNotFoundException : DomainException
{
    public BudgetNotFoundException(Guid budgetId)
        : base($"Budget with ID {budgetId} was not found.") { }
}

public sealed class BudgetAmountNegativeException : DomainException
{
    public BudgetAmountNegativeException(decimal budgetAmount)
        : base($"The budget amount {budgetAmount} cannot be negative.") { }
}

public sealed class BudgetNameEmptyException : DomainException
{
    public BudgetNameEmptyException()
        : base("The budget name cannot be empty.") { }
}
