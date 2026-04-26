using MediatR;
using personal.transaction.management.application.Reports.Dtos;

namespace personal.transaction.management.application.Reports.Queries.GetExpensesByCategory;

public record GetExpensesByCategoryQuery(Guid UserId, DateOnly From, DateOnly To) : IRequest<ExpensesByCategoryDto>;
