using MediatR;
using personal.transaction.management.application.Common.Exceptions;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Tags.Commands.RenameTag;

public sealed class RenameTagCommandHandler(ITagRepository tagRepository, IUnitOfWork unitOfWork) : IRequestHandler<RenameTagCommand>
{
	public async Task Handle(RenameTagCommand request, CancellationToken cancellationToken)
	{
		var tag = await tagRepository.GetByIdAsync(request.TagId, cancellationToken)
			?? throw new NotFoundException(nameof(Tag), request.TagId);

		// Domain enforces system tag guard + ownership check
		tag.Rename(request.UserId, request.Name);

		await unitOfWork.SaveChangesAsync(cancellationToken);
	}
}
