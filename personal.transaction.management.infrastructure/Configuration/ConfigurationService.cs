using Microsoft.Extensions.Options;
using personal.transaction.management.application.Common.Interfaces;

namespace personal.transaction.management.infrastructure.Configuration;

internal sealed class ConfigurationService(IOptions<ApplicationSettings> options) : IConfigurationService
{
    private readonly ApplicationSettings.SpendingSettings _spending = options.Value.Spending;

    public decimal SpendingAnomalyThreshold => _spending.AnomalyThreshold;
    public int SpendingMaxMonths => _spending.MaxMonths;
}
