using personal.transaction.management.domain.entities;

namespace personal.transaction.management.domain.repositories;

public interface ITagRepository : IRepository<Tag>
{
	Task<IReadOnlyList<Tag>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
	Task<IReadOnlyList<Tag>> GetSystemTagsAsync(CancellationToken cancellationToken = default);
	Task<bool> IsInUseAsync(Guid tagId, CancellationToken cancellationToken = default);
}
