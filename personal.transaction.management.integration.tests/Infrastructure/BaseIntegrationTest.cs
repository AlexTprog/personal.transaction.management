using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace personal.transaction.management.integration.tests.Infrastructure;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
	protected static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

	protected readonly HttpClient Client;
	protected readonly IntegrationTestWebAppFactory Factory;

	protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
	{
		Factory = factory;
		Client = factory.CreateClient();
	}

	protected async Task<(string Token, Guid UserId)> RegisterAndLoginAsync()
	{
		var email = $"test-{Guid.NewGuid()}@test.com";

		var registerResponse = await Client.PostAsJsonAsync("/api/auth/register", new
		{
			Email = email,
			FullName = "Integration Test User",
			Password = "Test1234!"
		}, JsonOptions);

		registerResponse.EnsureSuccessStatusCode();

		var auth = await registerResponse.Content.ReadFromJsonAsync<AuthResponse>(JsonOptions);
		Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.Token);
		return (auth.Token, auth.UserId);
	}

	protected async Task<Guid> CreateAccountAsync(decimal initialBalance, string currency = "USD", int accountType = 2)
	{
		var response = await Client.PostAsJsonAsync("/api/accounts", new
		{
			Name = "Test Account",
			AccountType = accountType,
			Currency = currency,
			Amount = initialBalance
		}, JsonOptions);
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadFromJsonAsync<Guid>(JsonOptions);
	}

	protected async Task<Guid> GetFirstCategoryIdAsync()
	{
		var response = await Client.GetAsync("/api/categories");
		response.EnsureSuccessStatusCode();

		var categories = await response.Content.ReadFromJsonAsync<List<CategoryResponse>>(JsonOptions);
		return categories!.First().Id;
	}

	protected async Task<AccountResponse> GetAccountAsync(Guid accountId)
	{
		var response = await Client.GetAsync($"/api/accounts/{accountId}");
		response.EnsureSuccessStatusCode();
		return (await response.Content.ReadFromJsonAsync<AccountResponse>(JsonOptions))!;
	}

	private record AuthResponse(Guid UserId, string Token);
	protected record AccountResponse(Guid Id, decimal Balance, string Currency);
	private record CategoryResponse(Guid Id, string Name);
}
