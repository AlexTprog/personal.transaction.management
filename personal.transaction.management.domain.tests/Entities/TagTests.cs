using personal.transaction.management.domain.entities;
using personal.transaction.management.domain.exceptions;

namespace personal.transaction.management.domain.tests.Entities;

public class TagTests
{
    [Fact]
    public void CreateUserTag_Should_InitializeNonSystemTag()
    {
        var userId = Guid.NewGuid();

        var tag = Tag.CreateUserTag(userId, "  vacation ");

        Assert.NotEqual(Guid.Empty, tag.Id);
        Assert.Equal(userId, tag.UserId);
        Assert.Equal("vacation", tag.Name);
        Assert.False(tag.IsSystem);
    }

    [Fact]
    public void CreateSystemTag_Should_InitializeSystemTagWithoutUser()
    {
        var tag = Tag.CreateSystemTag(" recurring ");

        Assert.Null(tag.UserId);
        Assert.Equal("recurring", tag.Name);
        Assert.True(tag.IsSystem);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void CreateUserTag_Should_Throw_When_NameIsEmpty(string? name)
    {
        var ex = Assert.Throws<DomainValidationException>(() => Tag.CreateUserTag(Guid.NewGuid(), name!));

        Assert.Equal("Name", ex.Field);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateSystemTag_Should_Throw_When_NameIsEmpty(string name)
    {
        Assert.Throws<DomainValidationException>(() => Tag.CreateSystemTag(name));
    }

    [Fact]
    public void Rename_Should_UpdateName_When_RequestedByOwner()
    {
        var userId = Guid.NewGuid();
        var tag = Tag.CreateUserTag(userId, "old");

        tag.Rename(userId, "  new ");

        Assert.Equal("new", tag.Name);
    }

    [Fact]
    public void Rename_Should_Throw_When_TagIsSystem()
    {
        var tag = Tag.CreateSystemTag("recurring");

        Assert.Throws<SystemTagModificationException>(() => tag.Rename(Guid.NewGuid(), "other"));
        Assert.Equal("recurring", tag.Name);
    }

    [Fact]
    public void Rename_Should_Throw_When_RequestedByAnotherUser()
    {
        var tag = Tag.CreateUserTag(Guid.NewGuid(), "old");

        Assert.Throws<UnauthorizedTagAccessException>(() => tag.Rename(Guid.NewGuid(), "new"));
        Assert.Equal("old", tag.Name);
    }

    [Fact]
    public void Rename_Should_Throw_When_NameIsEmpty()
    {
        var userId = Guid.NewGuid();
        var tag = Tag.CreateUserTag(userId, "old");

        Assert.Throws<DomainValidationException>(() => tag.Rename(userId, "  "));
        Assert.Equal("old", tag.Name);
    }
}
