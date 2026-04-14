using MediatR;
using personal.transaction.management.application.Common.Exceptions;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.exceptions;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Tags.Commands.DeleteTag;

public sealed class DeleteTagCommandHandler(ITagRepository tagRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteTagCommand>
{
	public async Task Handle(DeleteTagCommand request, CancellationToken cancellationToken)
	{
		var tag = await tagRepository.GetByIdAsync(request.TagId, cancellationToken)
			?? throw new NotFoundException(nameof(Tag), request.TagId);

		if (tag.IsSystem)
			throw new SystemTagModificationException();

		if (tag.UserId != request.UserId)
			throw new NotFoundException(nameof(Tag), request.TagId); // 404 to avoid info leakage

		if (await tagRepository.IsInUseAsync(request.TagId, cancellationToken))
			throw new ConflictException("Cannot delete a tag that is currently in use by transactions.");

		tagRepository.Remove(tag);
		await unitOfWork.SaveChangesAsync(cancellationToken);
	}
}
