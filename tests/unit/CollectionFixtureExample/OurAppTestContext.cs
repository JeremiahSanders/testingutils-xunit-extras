using FluentAssertions;

namespace Jds.TestingUtils.Xunit2.Extras.Tests.Unit.CollectionFixtureExample;

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

[CollectionDefinition(nameof(OurAppTestContext))]
public class OurTestAppContextCollection : ICollectionFixture<OurAppTestContext>
{
  // This class has no code, and is never created. Its purpose is simply
  // to be the place to apply [CollectionDefinition] and all the
  // ICollectionFixture<> interfaces.
}

/// <summary>
///   This is an example of how a test class which has no &quot;fixture&quot;.
///   I.e., it depends only upon the &quot;shared context&quot;.
/// </summary>
[Collection(nameof(OurAppTestContext))]
public class ExampleTestClassWithNoFixture
{
  private readonly OurAppTestContext _fixture;

  public ExampleTestClassWithNoFixture(OurAppTestContext fixture)
  {
    _fixture = fixture;
  }

  [Fact]
  public void SharedContextHasConfiguredUris()
  {
    _fixture.ConfiguredUris.Should().NotBeEmpty();
  }
}

/// <summary>
///   This is an example of a class-level fixture.
///   This is used to execute a particular arrangement once for all test methods in a &quot;test assertion class&quot;
///   (a class which uses it as the generic argument when implementing <see cref="IClassFixture{TFixture}" />).
/// </summary>
/// <remarks>
///   Note that this class does not have the <see cref="Xunit.CollectionAttribute" /> attribute.
///   Instead, the &quot;test assertion class&quot;
/// </remarks>
public class ExampleFixture
{
  public ExampleFixture(OurAppTestContext context)
  {
    Context = context;
  }

  public OurAppTestContext Context { get; }
}

[Collection(nameof(OurAppTestContext))]
public class ExampleFixtureAssertions : IClassFixture<ExampleFixture>
{
  private readonly ExampleFixture _fixture;

  public ExampleFixtureAssertions(ExampleFixture fixture)
  {
    _fixture = fixture;
  }

  [Fact]
  public void SharedContextHasConfiguredUris()
  {
    _fixture.Context.ConfiguredUris.Should().NotBeEmpty();
  }
}
