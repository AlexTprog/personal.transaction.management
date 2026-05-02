namespace personal.transaction.management.infrastructure.Configuration;

public sealed class ApplicationSettings
{
    public const string SectionName = "Application";
    public SpendingSettings Spending { get; set; } = new();

    public sealed class SpendingSettings
    {
        public decimal AnomalyThreshold { get; set; } = 0.4m;
        public int MaxMonths { get; set; } = 11;
    }
}
