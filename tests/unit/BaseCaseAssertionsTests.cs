using Xunit.Abstractions;

namespace Jds.TestingUtils.Xunit2.Extras.Tests.Unit;

public class BaseCaseAssertionsTests : BaseCaseAssertions<BaseCaseAssertionsTests.CaseFixture>
{
    public BaseCaseAssertionsTests(CaseFixture caseArrangementFixture, ITestOutputHelper testOutputHelper)
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

    public class CaseFixture : ICaseArrangementFixture
    {
        public static bool FixtureWasInitialized { get; private set; }

        public Task ActAsync()
        {
            if (FixtureWasInitialized)
                throw new InvalidOperationException(
                    "Fixture was already initialized! We're running the case arrangement more than once!");

            FixtureWasInitialized = true; // Record that we've initialized the fixture.

            return Task.CompletedTask;
        }
    }
}