using MediatR;

namespace personal.transaction.management.application.Tags.Commands.RenameTag;

public record RenameTagCommand(Guid TagId, Guid UserId, string Name) : IRequest;
