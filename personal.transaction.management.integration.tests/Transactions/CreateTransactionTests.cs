using personal.transaction.management.integration.tests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace personal.transaction.management.integration.tests.Transactions;

public sealed class CreateTransactionTests(IntegrationTestWebAppFactory factory)
	: BaseIntegrationTest(factory)
{
	[Fact]
	public async Task CreateIncomeTransaction_IncreasesAccountBalance()
	{
		await RegisterAndLoginAsync();
		var accountId = await CreateAccountAsync(initialBalance: 100m);
		var categoryId = await GetFirstCategoryIdAsync();

		var response = await Client.PostAsJsonAsync("/api/transactions", new
		{
			AccountId = accountId,
			CategoryId = categoryId,
			Amount = 250m,
			Currency = "USD",
			TransactionType = 1, // Income
			Date = DateOnly.FromDateTime(DateTime.Today)
		}, JsonOptions);

		Assert.Equal(HttpStatusCode.Created, response.StatusCode);

		var account = await GetAccountAsync(accountId);
		Assert.Equal(350m, account.Balance);
	}

	[Fact]
	public async Task CreateExpenseTransaction_DecreasesAccountBalance()
	{
		await RegisterAndLoginAsync();
		var accountId = await CreateAccountAsync(initialBalance: 500m);
		var categoryId = await GetFirstCategoryIdAsync();

		var response = await Client.PostAsJsonAsync("/api/transactions", new
		{
			AccountId = accountId,
			CategoryId = categoryId,
			Amount = 150m,
			Currency = "USD",
			TransactionType = 2, // Expense
			Date = DateOnly.FromDateTime(DateTime.Today)
		}, JsonOptions);

		Assert.Equal(HttpStatusCode.Created, response.StatusCode);

		var account = await GetAccountAsync(accountId);
		Assert.Equal(350m, account.Balance);
	}

	[Fact]
	public async Task CreateExpenseTransaction_ExceedingCashBalance_Returns422()
	{
		await RegisterAndLoginAsync();
		var accountId = await CreateAccountAsync(initialBalance: 50m, accountType: 1); // Cash
		var categoryId = await GetFirstCategoryIdAsync();

		var response = await Client.PostAsJsonAsync("/api/transactions", new
		{
			AccountId = accountId,
			CategoryId = categoryId,
			Amount = 200m,
			Currency = "USD",
			TransactionType = 2, // Expense
			Date = DateOnly.FromDateTime(DateTime.Today)
		}, JsonOptions);

		Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);

		// Verify balance was NOT changed
		var account = await GetAccountAsync(accountId);
		Assert.Equal(50m, account.Balance);
	}
}
