using FluentAssertions;
using Xunit.Abstractions;

namespace Jds.TestingUtils.Xunit2.Extras.Tests.Unit.Examples.WithExtras.SharedConfiguration;

/// <summary>
///   An example &quot;shared context&quot; <c>class</c> which shares configuration or initialization across multiple
///   test case arrangement fixtures.
/// </summary>
/// <remarks>
///   <para>
///     In this example we're defining an asynchronously-obtained <see cref="SharedAsynchronousValue" /> which is
///     important to our test case arrangement fixtures.
///   </para>
/// </remarks>
[SharedCaseContext]
public class ExampleSharedContext : IAsyncLifetime
{
  /// <summary>
  ///   Gets a shared value needed across multiple tests.
  /// </summary>
  /// <remarks>
  ///   <para>
  ///     In a real test this might be...
  ///   </para>
  ///   <para>
  ///     ...an authorization token which needs to be applied to request headers.
  ///   </para>
  ///   <para>
  ///     ...configuration values which could only be obtained asynchronously (e.g., from a remote source, like AWS SSM).
  ///   </para>
  /// </remarks>
  public Guid SharedAsynchronousValue { get; private set; }

  public async Task DisposeAsync()
  {
    await Task.Delay(1); // Simulating an async cleanup step
  }

  public async Task InitializeAsync()
  {
    await Task.Delay(1); // Simulating an async initialization step

    SharedAsynchronousValue = Guid.NewGuid();
  }
}

/// <summary>
///   An example test case arrangement &quot;fixture&quot;.
/// </summary>
/// <remarks>
///   <para>
///     When <c>xUnit</c> runs the tests contained in <see cref="ExampleSharedContextArrangementAssertions" /> it will
///     only create a single instance of this arrangement.
///   </para>
///   <para>
///     Additionally, the auto-generated <see cref="ExampleSharedContextFixture" /> implements
///     <see cref="ICaseArrangementFixture" />.
///     That base class requires this class to implement <see cref="ActAsync" /> (the core work which this test case
///     arrangement encapsulates).
///   </para>
/// </remarks>
public class ExampleSharedContextArrangement : ExampleSharedContextFixture
{
  public ExampleSharedContextArrangement(ExampleSharedContext context) : base(context)
  {
  }

  public string ActResult { get; private set; } = string.Empty;

  public Guid VerificationValue { get; private set; }

  protected override async Task ActAsync()
  {
    await Task.Delay(1); // Simulating an async action, like making an HTTP request.
    ActResult = Context.SharedAsynchronousValue.ToString("D");
  }

  protected override Task AcquireVerificationValuesAsync()
  {
    if (Guid.TryParse(ActResult, out var result))
    {
      VerificationValue = result;
    }

    return Task.CompletedTask;
  }
}

/// <summary>
///   An example test class which makes assertions upon the actions arranged in its fixture,
///   <see cref="ExampleSharedContextArrangement" />.
/// </summary>
public class ExampleSharedContextArrangementAssertions : ExampleSharedContextAssertions<ExampleSharedContextArrangement>
{
  public ExampleSharedContextArrangementAssertions(ExampleSharedContextArrangement caseArrangementFixture,
    ITestOutputHelper testOutputHelper)
    : base(caseArrangementFixture, testOutputHelper)
  {
  }

  [Fact]
  public void ObtainedExpectedResult()
  {
    var expected = CaseArrangement.Context.SharedAsynchronousValue.ToString("D");

    CaseArrangement.ActResult.Should().Be(expected);
  }
}

/// <summary>
///   An example test class which does not need a single-initialization fixture,
///   but which still needs information from the shared test context.
/// </summary>
public class AssertionsWhichDependUponTheSharedContextButNeedsNoFixture : ExampleSharedContextAssertions
{
  public AssertionsWhichDependUponTheSharedContextButNeedsNoFixture(ExampleSharedContextFixture fixture,
    ITestOutputHelper testOutputHelper)
    : base(fixture, testOutputHelper)
  {
  }

  /// <summary>
  ///   A simple example assertion. Uses information from the read-only shared context.
  /// </summary>
  [Fact]
  public void ContextIsInitialized()
  {
    CaseContext.SharedAsynchronousValue.Should().NotBe(Guid.Empty);
  }
}
