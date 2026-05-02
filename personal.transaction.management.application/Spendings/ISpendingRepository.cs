using personal.transaction.management.application.Spendings.Dtos;

namespace personal.transaction.management.application.Spendings;

public interface ISpendingRepository
{
	Task<ICollection<SpendingCategoryDto>> GetSpendingByCategoriesAsync(Guid userId, DateOnly from, CancellationToken cancellationToken = default);
	Task<ICollection<SpendingCategoryAverageDto>> GetSpendingAverageByCategoriesAsync(Guid userId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default);
}
