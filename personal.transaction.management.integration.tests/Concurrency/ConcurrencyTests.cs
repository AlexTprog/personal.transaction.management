using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using personal.transaction.management.integration.tests.Infrastructure;

namespace personal.transaction.management.integration.tests.Concurrency;

public sealed class ConcurrencyTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task TwoConcurrentExpenses_BothSucceed_BalancereflectsBothDebits()
    {
        var (token, _) = await RegisterAndLoginAsync();

        // Bank account (no insufficient-funds enforcement) so both can succeed
        var accountId = await CreateAccountAsync(initialBalance: 1000m, accountType: 2);
        var categoryId = await GetFirstCategoryIdAsync();

        // Two clients sharing the same authenticated user
        using var client2 = Factory.CreateClient();
        client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var request = new
        {
            AccountId = accountId,
            CategoryId = categoryId,
            Amount = 300m,
            Currency = "USD",
            TransactionType = 2, // Expense
            Date = DateOnly.FromDateTime(DateTime.Today)
        };

        // Fire both requests concurrently — one will hit a concurrency conflict and retry
        var task1 = Client.PostAsJsonAsync("/api/transactions", request, JsonOptions);
        var task2 = client2.PostAsJsonAsync("/api/transactions", request, JsonOptions);

        var results = await Task.WhenAll(task1, task2);

        Assert.All(results, r => Assert.Equal(HttpStatusCode.Created, r.StatusCode));

        var account = await GetAccountAsync(accountId);
        Assert.Equal(400m, account.Balance); // 1000 - 300 - 300
    }

    [Fact]
    public async Task ThreeConcurrentExpenses_ExhaustingRetries_AtLeastOneSucceeds()
    {
        var (token, _) = await RegisterAndLoginAsync();

        var accountId = await CreateAccountAsync(initialBalance: 1000m, accountType: 2);
        var categoryId = await GetFirstCategoryIdAsync();

        using var client2 = Factory.CreateClient();
        using var client3 = Factory.CreateClient();
        client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        client3.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var request = new
        {
            AccountId = accountId,
            CategoryId = categoryId,
            Amount = 100m,
            Currency = "USD",
            TransactionType = 2,
            Date = DateOnly.FromDateTime(DateTime.Today)
        };

        var results = await Task.WhenAll(
            Client.PostAsJsonAsync("/api/transactions", request, JsonOptions),
            client2.PostAsJsonAsync("/api/transactions", request, JsonOptions),
            client3.PostAsJsonAsync("/api/transactions", request, JsonOptions));

        // At least one succeeded; any conflicts that exceeded retries return 409
        Assert.Contains(results, r => r.StatusCode == HttpStatusCode.Created);
        Assert.All(results, r =>
            Assert.True(r.StatusCode is HttpStatusCode.Created or HttpStatusCode.Conflict));

        var account = await GetAccountAsync(accountId);
        var succeeded = results.Count(r => r.StatusCode == HttpStatusCode.Created);
        Assert.Equal(1000m - (100m * succeeded), account.Balance);
    }
}
