using FluentAssertions;

namespace Jds.TestingUtils.Xunit2.Extras.Tests.Unit.Examples.XunitOnly.CollectionFixtureExample;

/// <summary>
///   This is an example of a &quot;shared test context&quot;.
///   I.e., a class which provides shared <c>readonly</c> configuration or context across multiple test classes.
/// </summary>
public class OurAppTestContext
{
  public OurAppTestContext()
  {
    ConfiguredUris = new Dictionary<string, Uri>
    {
      ["client-a"] = new("https://client-a/", UriKind.Absolute),
      ["client-b"] = new("https://client-b/", UriKind.Absolute)
    };
  }

  public IReadOnlyDictionary<string, Uri> ConfiguredUris { get; }
}

/// <summary>
///   This is a class which xUnit requires to support constructor dependency injection
///   of the &quot;shared test context&quot;.
///   According to xUnit documentation:
///   This class has no code, and is never created. Its purpose is simply
///   to be the place to apply <see cref="CollectionDefinitionAttribute" /> and all the
///   <see cref="ICollectionFixture{TFixture}" /> interfaces.
/// </summary>
[CollectionDefinition(nameof(OurAppTestContext))]
public class OurTestAppContextCollection : ICollectionFixture<OurAppTestContext>
{
}

/// <summary>
///   This is an example of how a test class which has no &quot;fixture&quot;.
///   I.e., it depends only upon the &quot;shared context&quot;.
/// </summary>
[Collection(nameof(OurAppTestContext))]
public class ExampleTestClassWithNoFixture(OurAppTestContext fixture)
{
  [Fact]
  public void SharedContextHasConfiguredUris()
  {
    fixture.ConfiguredUris.Should().NotBeEmpty();
  }
}

/// <summary>
///   This is an example of a class-level fixture.
///   This is used to execute a particular arrangement once for all test methods in a &quot;test assertion class&quot;
///   (a class which uses it as the generic argument when implementing <see cref="IClassFixture{TFixture}" />).
/// </summary>
/// <remarks>
///   Note that this class does NOT have the <see cref="Xunit.CollectionAttribute" /> attribute.
///   Instead, the &quot;test assertion class&quot;
/// </remarks>
public class ExampleFixture(OurAppTestContext context)
{
  public OurAppTestContext Context { get; } = context;
}

/// <summary>
///   This is an example of a &quot;test assertion class&quot;.
/// </summary>
/// <remarks>
///   <para>
///     Note that this class uses <see cref="IClassFixture{TFixture}" /> so that a single instance of its fixture
///     is used for every test method.
///     Additionally, this class has the <see cref="Xunit.CollectionAttribute" /> attribute identifying it as depending
///     upon the shared test context on which its case arrangement depends.
///   </para>
/// </remarks>
[Collection(nameof(OurAppTestContext))]
public class ExampleFixtureAssertions(ExampleFixture fixture) : IClassFixture<ExampleFixture>
{
  [Fact]
  public void SharedContextHasConfiguredUris()
  {
    fixture.Context.ConfiguredUris.Should().NotBeEmpty();
  }
}
