using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.exceptions;

namespace personal.transaction.management.domain.tests.Entities;

public class UserTests
{
    private static User CreateUser() => User.Create("john@example.com", "John Doe", "hash");

    [Fact]
    public void Create_Should_InitializeUser_When_DataIsValid()
    {
        var user = User.Create("  John@Example.com ", "  John Doe ", "hashed-pwd");

        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal("john@example.com", user.Email.Value);
        Assert.Equal("John Doe", user.FullName);
        Assert.Equal("hashed-pwd", user.PasswordHash);
        Assert.True(user.IsActive);
        Assert.Empty(user.Accounts);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_Should_Throw_When_FullNameIsEmpty(string fullName)
    {
        var ex = Assert.Throws<DomainValidationException>(() => User.Create("a@b.com", fullName, "hash"));

        Assert.Equal("FullName", ex.Field);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_Should_Throw_When_PasswordHashIsEmpty(string passwordHash)
    {
        var ex = Assert.Throws<DomainValidationException>(() => User.Create("a@b.com", "John", passwordHash));

        Assert.Equal("PasswordHash", ex.Field);
    }

    [Fact]
    public void Create_Should_Throw_When_EmailIsInvalid()
    {
        var ex = Assert.Throws<DomainValidationException>(() => User.Create("not-an-email", "John", "hash"));

        Assert.Equal("Email", ex.Field);
    }

    [Fact]
    public void UpdateProfile_Should_UpdateTrimmedFullName()
    {
        var user = CreateUser();

        user.UpdateProfile("  Jane Doe ");

        Assert.Equal("Jane Doe", user.FullName);
    }

    [Fact]
    public void UpdateProfile_Should_Throw_When_FullNameIsEmpty()
    {
        var user = CreateUser();

        Assert.Throws<DomainValidationException>(() => user.UpdateProfile(" "));
        Assert.Equal("John Doe", user.FullName);
    }

    [Fact]
    public void ChangePassword_Should_UpdatePasswordHash()
    {
        var user = CreateUser();

        user.ChangePassword("new-hash");

        Assert.Equal("new-hash", user.PasswordHash);
    }

    [Fact]
    public void ChangePassword_Should_Throw_When_HashIsEmpty()
    {
        var user = CreateUser();

        var ex = Assert.Throws<DomainValidationException>(() => user.ChangePassword(""));

        Assert.Equal("PasswordHash", ex.Field);
        Assert.Equal("hash", user.PasswordHash);
    }

    [Fact]
    public void Deactivate_Should_SetIsActiveToFalse()
    {
        var user = CreateUser();

        user.Deactivate();

        Assert.False(user.IsActive);
    }
}
