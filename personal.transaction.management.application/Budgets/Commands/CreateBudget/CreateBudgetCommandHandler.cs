using MediatR;
using personal.transaction.management.domain.Entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Budgets.Commands.CreateBudget;

public class CreateBudgetCommandHandler(
    IUnitOfWork unitOfWork,
    IBudgetRepository budgetRepository) : IRequestHandler<CreateBudgetCommand, Guid>
{
    public async Task<Guid> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        var budget = Budget.Create(
            request.UserId,
            request.CategoryId,
            request.StartDate,
            request.EndDate,
            request.Name,
            request.Amount);

        await budgetRepository.AddAsync(budget);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return budget.Id;
    }
}
