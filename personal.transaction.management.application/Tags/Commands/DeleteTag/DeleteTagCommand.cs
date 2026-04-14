using MediatR;

namespace personal.transaction.management.application.Tags.Commands.DeleteTag;

public record DeleteTagCommand(Guid TagId, Guid UserId) : IRequest;
