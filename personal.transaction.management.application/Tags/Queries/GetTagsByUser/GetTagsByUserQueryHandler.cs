using MediatR;
using personal.transaction.management.application.Tags.Dtos;
using personal.transaction.management.domain.repositories;

namespace personal.transaction.management.application.Tags.Queries.GetTagsByUser;

public sealed class GetTagsByUserQueryHandler(ITagRepository tagRepository) : IRequestHandler<GetTagsByUserQuery, IReadOnlyList<TagDto>>
{
	public async Task<IReadOnlyList<TagDto>> Handle(GetTagsByUserQuery request, CancellationToken cancellationToken)
	{
		var tags = await tagRepository.GetByUserIdAsync(request.UserId, cancellationToken);
		return tags.Select(TagDto.FromEntity).ToList();
	}
}
