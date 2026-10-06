using personal.transaction.management.domain.exceptions;
using personal.transaction.management.domain.valueobjects;

namespace personal.transaction.management.domain.tests.ValueObjects;

public class HexColorTests
{
    [Theory]
    [InlineData("#FFFFFF", "#FFFFFF")]
    [InlineData("#a1b2c3", "#A1B2C3")]
    [InlineData("#000000", "#000000")]
    public void From_Should_NormalizeToUpperCase_When_ColorIsValid(string input, string expected)
    {
        var color = HexColor.From(input);

        Assert.Equal(expected, color.Value);
        Assert.Equal(expected, color.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void From_Should_Throw_When_ColorIsEmpty(string? input)
    {
        var ex = Assert.Throws<DomainValidationException>(() => HexColor.From(input!));

        Assert.Equal("Color", ex.Field);
    }

    [Theory]
    [InlineData("FFFFFF")]
    [InlineData("#FFF")]
    [InlineData("#FFFFFFF")]
    [InlineData("#GGGGGG")]
    [InlineData(" #FFFFFF")]
    public void From_Should_Throw_When_ColorIsInvalid(string input)
    {
        var ex = Assert.Throws<DomainValidationException>(() => HexColor.From(input));

        Assert.Equal("Color", ex.Field);
    }
}
