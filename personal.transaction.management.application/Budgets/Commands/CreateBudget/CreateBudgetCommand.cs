using MediatR;

namespace personal.transaction.management.application.Budgets.Commands.CreateBudget;

public class CreateBudgetCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Amount { get; set; }
}
