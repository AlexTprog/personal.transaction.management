using MediatR;

namespace personal.transaction.management.application.Categories.Commands.DeactivateCategory;

public record DeactivateCategoryCommand(Guid CategoryId, Guid UserId) : IRequest;
