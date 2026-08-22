using System.Diagnostics;

namespace personal.transaction.management.application.Common.Diagnostics;

public static class ApplicationDiagnostics
{
    public const string ActivitySourceName = "PersonalTransactionManagement.Application";

    public static readonly ActivitySource ActivitySource = new(ActivitySourceName);
}
