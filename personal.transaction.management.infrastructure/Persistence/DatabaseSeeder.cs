using Microsoft.EntityFrameworkCore;
using personal.transaction.management.application.Common.Interfaces;
using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.enums;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.infrastructure.Persistence;

public sealed class DatabaseSeeder(ApplicationDbContext context, IPasswordHasher passwordHasher)
{
	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		await context.Database.MigrateAsync(cancellationToken);

		if (await context.Users.AnyAsync(cancellationToken))
			return;

		var user = User.Create("seed@test.com", "Seed User", passwordHasher.Hash("Seed1234!"));
		var categories = BuildCategories();

		context.Users.Add(user);
		context.Categories.AddRange(categories);
		await context.SaveChangesAsync(cancellationToken);

		var accounts = BuildAccounts(user.Id);
		context.Accounts.AddRange(accounts);
		await context.SaveChangesAsync(cancellationToken);

		var (income, expenses) = BuildTransactions(user.Id, accounts, categories);
		context.Transactions.AddRange(income);
		await context.SaveChangesAsync(cancellationToken);

		context.Transactions.AddRange(expenses);
		await context.SaveChangesAsync(cancellationToken);
	}

	private static List<Category> BuildCategories() =>
	[
		Category.CreateSystemCategory("Salary",        "work",            "#4CAF50", CategoryTypeEnum.Income),
		Category.CreateSystemCategory("Freelance",     "computer",        "#8BC34A", CategoryTypeEnum.Income),
		Category.CreateSystemCategory("Food",          "restaurant",      "#FF5722", CategoryTypeEnum.Expense),
		Category.CreateSystemCategory("Transport",     "directions_car",  "#2196F3", CategoryTypeEnum.Expense),
		Category.CreateSystemCategory("Entertainment", "movie",           "#9C27B0", CategoryTypeEnum.Expense),
		Category.CreateSystemCategory("Health",        "local_hospital",  "#F44336", CategoryTypeEnum.Expense),
		Category.CreateSystemCategory("Shopping",      "shopping_bag",    "#FF9800", CategoryTypeEnum.Expense),
		Category.CreateSystemCategory("Rent",          "home",            "#607D8B", CategoryTypeEnum.Expense),
	];

	private static List<Account> BuildAccounts(Guid userId) =>
	[
		Account.Create(userId, "Checking Account", AccountTypeEnum.Bank,    Money.Of(100m, "USD")),
		Account.Create(userId, "Digital Wallet",   AccountTypeEnum.Digital, Money.Of(50m,  "USD")),
	];

	private static (List<Transaction> Income, List<Transaction> Expenses) BuildTransactions(
		Guid userId, List<Account> accounts, List<Category> categories)
	{
		var checking = accounts[0];
		var wallet = accounts[1];

		var salary = categories.First(c => c.Name == "Salary");
		var freelance = categories.First(c => c.Name == "Freelance");
		var food = categories.First(c => c.Name == "Food");
		var transport = categories.First(c => c.Name == "Transport");
		var entertain = categories.First(c => c.Name == "Entertainment");
		var health = categories.First(c => c.Name == "Health");
		var shopping = categories.First(c => c.Name == "Shopping");
		var rent = categories.First(c => c.Name == "Rent");

		var today = DateOnly.FromDateTime(DateTime.UtcNow);
		var income = new List<Transaction>();
		var expenses = new List<Transaction>();

		for (int i = 5; i >= 0; i--)
		{
			var reference = today.AddMonths(-i);
			var y = reference.Year;
			var m = reference.Month;
			var label = reference.ToString("MMM yyyy");

			income.Add(Tx(checking, userId, salary, 3200m, TransactionTypeEnum.Income,
				$"Salary {label}", new DateOnly(y, m, 1)));

			if (i % 2 == 0)
				income.Add(Tx(wallet, userId, freelance, 800m, TransactionTypeEnum.Income,
					$"Freelance {label}", new DateOnly(y, m, 10)));

			expenses.Add(Tx(checking, userId, rent, 1200m, TransactionTypeEnum.Expense, "Monthly rent", new DateOnly(y, m, 3)));
			expenses.Add(Tx(checking, userId, transport, 90m, TransactionTypeEnum.Expense, "Transit pass", new DateOnly(y, m, 2)));
			expenses.Add(Tx(checking, userId, food, 160m, TransactionTypeEnum.Expense, "Supermarket", new DateOnly(y, m, 7)));
			expenses.Add(Tx(wallet, userId, food, 45m, TransactionTypeEnum.Expense, "Restaurant", new DateOnly(y, m, 14)));
			expenses.Add(Tx(wallet, userId, entertain, 60m, TransactionTypeEnum.Expense, "Cinema", new DateOnly(y, m, 16)));
			expenses.Add(Tx(checking, userId, health, 75m, TransactionTypeEnum.Expense, "Pharmacy", new DateOnly(y, m, 20)));

			if (i % 3 == 0)
				expenses.Add(Tx(wallet, userId, shopping, 220m, TransactionTypeEnum.Expense, "Clothing", new DateOnly(y, m, 22)));
		}

		return (income, expenses);
	}

	private static Transaction Tx(
		Account account, Guid userId, Category category,
		decimal amount, TransactionTypeEnum type, string description, DateOnly date)
		=> Transaction.Create(
			account.Id, userId, category.Id,
			Money.Of(amount, "USD"), type, description, date,
			transferId: null, exchangeRate: null, attachmentUrl: null);
}
