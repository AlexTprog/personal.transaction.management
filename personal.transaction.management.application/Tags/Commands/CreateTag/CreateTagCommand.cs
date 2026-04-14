using MediatR;

namespace personal.transaction.management.application.Tags.Commands.CreateTag;

public record CreateTagCommand(Guid UserId, string Name) : IRequest<Guid>;
