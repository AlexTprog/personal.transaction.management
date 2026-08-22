using System.Reflection;
using NetArchTest.Rules;

namespace personal.transaction.management.architecture.tests;

public class CleanArchitectureTests
{
    private static readonly Assembly DomainAssembly = typeof(personal.transaction.management.domain.entities.Transaction).Assembly;
    private static readonly Assembly ApplicationAssembly = typeof(personal.transaction.management.application.DependencyInjection).Assembly;
    private static readonly Assembly InfrastructureAssembly = typeof(personal.transaction.management.infrastructure.DependencyInjection).Assembly;
    private static readonly Assembly ApiAssembly = typeof(Program).Assembly;

    private const string DomainNamespace = "personal.transaction.management.domain";
    private const string ApplicationNamespace = "personal.transaction.management.application";
    private const string InfrastructureNamespace = "personal.transaction.management.infrastructure";
    private const string ApiNamespace = "personal.transaction.management.api";

    [Fact]
    public void Domain_Should_Not_HaveDependencyOn_Infrastructure()
    {
        var result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, Describe(result));
    }

    [Fact]
    public void Domain_Should_Not_HaveDependencyOn_Application()
    {
        var result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApplicationNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, Describe(result));
    }

    [Fact]
    public void Domain_Should_Not_HaveDependencyOn_Api()
    {
        var result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApiNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, Describe(result));
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOn_Infrastructure()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, Describe(result));
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOn_Api()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(ApiNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, Describe(result));
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOn_Api()
    {
        var result = Types.InAssembly(InfrastructureAssembly)
            .Should()
            .NotHaveDependencyOn(ApiNamespace)
            .GetResult();

        Assert.True(result.IsSuccessful, Describe(result));
    }

    private static string Describe(TestResult result) =>
        result.IsSuccessful
            ? string.Empty
            : "Types violating the rule: " + string.Join(", ", result.FailingTypeNames ?? []);
}
