using personal.transaction.management.application.Spendings.Dtos;

namespace personal.transaction.management.application.Spending;

public interface ISpendingRepository
{
	Task<ICollection<SpendingCategoryDto>> GetSpendingsByCategoryAsync(Guid userId, DateOnly from, CancellationToken cancellationToken = default);
	Task<ICollection<SpedingCategoryAverageDto>> GetSpedingAverageByCategoryAsync(Guid userId, DateOnly from, DateOnly to, CancellationToken cancellationToken = default);
}
