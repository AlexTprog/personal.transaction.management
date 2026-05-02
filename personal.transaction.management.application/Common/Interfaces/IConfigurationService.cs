namespace personal.transaction.management.application.Common.Interfaces;

public interface IConfigurationService
{
    decimal SpendingAnomalyThreshold { get; }
    int SpendingMaxMonths { get; }
}
