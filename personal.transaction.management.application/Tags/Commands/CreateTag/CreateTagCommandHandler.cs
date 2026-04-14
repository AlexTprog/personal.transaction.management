using MediatR;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Tags.Commands.CreateTag;

public sealed class CreateTagCommandHandler(ITagRepository tagRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateTagCommand, Guid>
{
	public async Task<Guid> Handle(CreateTagCommand request, CancellationToken cancellationToken)
	{
		var tag = Tag.CreateUserTag(request.UserId, request.Name);

		await tagRepository.AddAsync(tag, cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);

		return tag.Id;
	}
}
