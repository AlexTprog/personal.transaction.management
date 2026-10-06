using personal.transaction.management.domain.exceptions;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.domain.tests.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("john.doe@example.com", "john.doe@example.com")]
    [InlineData("John.Doe@Example.COM", "john.doe@example.com")]
    [InlineData("  user+tag@mail.co  ", "user+tag@mail.co")]
    public void From_Should_NormalizeEmail_When_EmailIsValid(string input, string expected)
    {
        var email = Email.From(input);

        Assert.Equal(expected, email.Value);
        Assert.Equal(expected, email.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void From_Should_Throw_When_EmailIsEmpty(string? input)
    {
        var ex = Assert.Throws<DomainValidationException>(() => Email.From(input!));

        Assert.Equal("Email", ex.Field);
    }

    [Theory]
    [InlineData("plainaddress")]
    [InlineData("@example.com")]
    [InlineData("user@")]
    [InlineData("user@example")]
    [InlineData("user@example.c")]
    [InlineData("user name@example.com")]
    public void From_Should_Throw_When_EmailIsInvalid(string input)
    {
        var ex = Assert.Throws<DomainValidationException>(() => Email.From(input));

        Assert.Equal("Email", ex.Field);
    }

    [Fact]
    public void ImplicitConversion_Should_ReturnValue()
    {
        string value = Email.From("a@b.com");

        Assert.Equal("a@b.com", value);
    }

    [Fact]
    public void Emails_Should_BeEqual_When_NormalizedValuesMatch()
    {
        Assert.Equal(Email.From("A@B.com"), Email.From("a@b.com"));
    }
}
