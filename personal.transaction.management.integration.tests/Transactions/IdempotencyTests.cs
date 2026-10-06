using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using personal.transaction.management.integration.tests.Infrastructure;

namespace personal.transaction.management.integration.tests.Transactions;

public sealed class IdempotencyTests(IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory)
{
    private const string IdempotencyKeyHeader = "Idempotency-Key";

    [Fact]
    public async Task RepeatedRequestWithSameKey_IsAppliedOnce_AndReturnsSameId()
    {
        await RegisterAndLoginAsync();
        var accountId = await CreateAccountAsync(initialBalance: 1000m);
        var categoryId = await GetFirstCategoryIdAsync();
        var key = Guid.NewGuid();
        var body = ExpenseBody(accountId, categoryId, 300m);

        var first = await PostWithKeyAsync(Client, body, key);
        var second = await PostWithKeyAsync(Client, body, key);

        Assert.Equal(HttpStatusCode.Created, first.StatusCode);
        Assert.Equal(HttpStatusCode.Created, second.StatusCode);
        Assert.Equal(
            await first.Content.ReadFromJsonAsync<Guid>(),
            await second.Content.ReadFromJsonAsync<Guid>());

        var account = await GetAccountAsync(accountId);
        Assert.Equal(700m, account.Balance);
    }

    [Fact]
    public async Task ConcurrentRequestsWithSameKey_AreAppliedOnce()
    {
        var (token, _) = await RegisterAndLoginAsync();
        var accountId = await CreateAccountAsync(initialBalance: 1000m);
        var categoryId = await GetFirstCategoryIdAsync();
        var key = Guid.NewGuid();
        var body = ExpenseBody(accountId, categoryId, 100m);

        using var client2 = Factory.CreateClient();
        using var client3 = Factory.CreateClient();
        client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        client3.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var results = await Task.WhenAll(
            PostWithKeyAsync(Client, body, key),
            PostWithKeyAsync(client2, body, key),
            PostWithKeyAsync(client3, body, key));

        Assert.All(results, r => Assert.Equal(HttpStatusCode.Created, r.StatusCode));

        var ids = await Task.WhenAll(results.Select(r => r.Content.ReadFromJsonAsync<Guid>()));
        Assert.Single(ids.Distinct());

        var account = await GetAccountAsync(accountId);
        Assert.Equal(900m, account.Balance);
    }

    [Fact]
    public async Task SameKeyWithDifferentPayload_Returns422()
    {
        await RegisterAndLoginAsync();
        var accountId = await CreateAccountAsync(initialBalance: 1000m);
        var categoryId = await GetFirstCategoryIdAsync();
        var key = Guid.NewGuid();

        var first = await PostWithKeyAsync(Client, ExpenseBody(accountId, categoryId, 100m), key);
        var second = await PostWithKeyAsync(Client, ExpenseBody(accountId, categoryId, 200m), key);

        Assert.Equal(HttpStatusCode.Created, first.StatusCode);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, second.StatusCode);

        var account = await GetAccountAsync(accountId);
        Assert.Equal(900m, account.Balance);
    }

    [Fact]
    public async Task FailedRequest_DoesNotConsumeKey()
    {
        await RegisterAndLoginAsync();
        var accountId = await CreateAccountAsync(initialBalance: 1000m);
        var categoryId = await GetFirstCategoryIdAsync();
        var key = Guid.NewGuid();
        var body = ExpenseBody(accountId, Guid.NewGuid(), 100m); // unknown category -> 404

        var failed = await PostWithKeyAsync(Client, body, key);
        Assert.Equal(HttpStatusCode.NotFound, failed.StatusCode);

        var retried = await PostWithKeyAsync(Client, ExpenseBody(accountId, categoryId, 100m), key);
        Assert.Equal(HttpStatusCode.Created, retried.StatusCode);
    }

    private static object ExpenseBody(Guid accountId, Guid categoryId, decimal amount) => new
    {
        AccountId = accountId,
        CategoryId = categoryId,
        Amount = amount,
        Currency = "USD",
        TransactionType = 2, // Expense
        Date = DateOnly.FromDateTime(DateTime.Today)
    };

    private static Task<HttpResponseMessage> PostWithKeyAsync(HttpClient client, object body, Guid key)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/transactions")
        {
            Content = JsonContent.Create(body, options: JsonOptions)
        };
        request.Headers.Add(IdempotencyKeyHeader, key.ToString());
        return client.SendAsync(request);
    }
}
