using MediatR;
using personal.transaction.management.application.Tags.Dtos;

namespace personal.transaction.management.application.Tags.Queries.GetTagsByUser;

public record GetTagsByUserQuery(Guid UserId) : IRequest<IReadOnlyList<TagDto>>;
