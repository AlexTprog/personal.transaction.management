using personal.transaction.management.domain.abstractions;
using personal.transaction.management.domain.enums;
using personal.transaction.management.domain.exceptions;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.domain.entities;

public class Category : BaseAuditable
{
	public Guid Id { get; private set; }
	public Guid? UserId { get; private set; }
	public string Name { get; private set; } = string.Empty;
	public string Icon { get; private set; } = string.Empty;
	public HexColor Color { get; private set; } = null!;
	public CategoryTypeEnum CategoryType { get; private set; }
	public bool IsSystem { get; private set; }
	public bool IsActive { get; private set; }

	private Category() { }

	private Category(
		Guid? userId, string name, string icon, HexColor color,
		CategoryTypeEnum categoryType)
	{
		Id = Guid.NewGuid();
		UserId = userId;
		Name = name;
		Icon = icon;
		Color = color;
		CategoryType = categoryType;
		IsSystem = userId == null;
		IsActive = true;
	}

	public static Category CreateUserCategory(
		Guid userId, string name, string icon,
		string hexColor, CategoryTypeEnum categoryType)
	{
		ValidateNameAndIcon(name, icon);
		return new Category(userId, name.Trim(), icon.Trim(), HexColor.From(hexColor), categoryType);
	}

	public static Category CreateSystemCategory(
		string name, string icon,
		string hexColor, CategoryTypeEnum categoryType)
	{
		ValidateNameAndIcon(name, icon);
		return new Category(null, name.Trim(), icon.Trim(), HexColor.From(hexColor), categoryType);
	}

	public void Update(string name, string icon, string hexColor, CategoryTypeEnum categoryType)
	{
		if (IsSystem)
			throw new SystemCategoryModificationException();

		ValidateNameAndIcon(name, icon);

		Name = name.Trim();
		Icon = icon.Trim();
		Color = HexColor.From(hexColor);
		CategoryType = categoryType;
	}

	public void Deactivate()
	{
		if (IsSystem)
			throw new SystemCategoryDeactivationException();

		IsActive = false;
	}

	private static void ValidateNameAndIcon(string name, string icon)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new DomainValidationException("Name", "Category name cannot be empty.");

		if (string.IsNullOrWhiteSpace(icon))
			throw new DomainValidationException("Icon", "Category icon cannot be empty.");
	}
}
