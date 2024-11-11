using Xunit.Abstractions;

namespace Jds.TestingUtils.Xunit2.Extras.Tests.Unit;

public class BaseCaseFixtureTests : BaseCaseAssertions<BaseCaseFixtureTests.CaseFixture>
{
  public BaseCaseFixtureTests(CaseFixture caseArrangementFixture, ITestOutputHelper testOutputHelper)
    : base(caseArrangementFixture, testOutputHelper)
  {
  }

  [Fact]
  public void CaseArrangementIsCreated()
  {
    Assert.NotNull(CaseArrangement);
    Assert.IsType<CaseFixture>(CaseArrangement);
  }

  [Fact]
  public void FixtureIsInitialized()
  {
    Assert.True(CaseFixture.FixtureWasInitialized);
  }

  public class CaseFixture : BaseCaseFixture
  {
    public static bool FixtureWasInitialized { get; private set; }

    protected override Task ActAsync()
    {
      if (FixtureWasInitialized)
      {
        throw new InvalidOperationException(
          "Fixture was already initialized! We're running the case arrangement more than once!");
      }

      FixtureWasInitialized = true; // Record that we've initialized the fixture.

      return Task.CompletedTask;
    }
  }
}
