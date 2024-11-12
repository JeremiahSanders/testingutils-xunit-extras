namespace Jds.TestingUtils.Xunit2.Extras.Tests.Unit.Examples.WithExtras.SingleInitialization;

/// <summary>
///   This class is an example representing a test case where the case arrangement workflow is executed once.
///   Test methods then perform assertions upon the completed test case results.
/// </summary>
/// <remarks>
///   <para>
///     This pattern is very useful in situations where the case arrangement is complex or resource intensive.
///   </para>
///   <para>Examples:</para>
///   <para>
///     ASP.NET Core integration tests:
///     Use the case arrangement fixture's <see cref="ICasePhases.ActAsync" /> to perform an API request.
///     Use the fixture's <see cref="ICasePhases.AcquireVerificationValuesAsync" /> to read the API response
///     <see cref="HttpResponseMessage.Content" />.
///     Use the fixture's <see cref="ICasePhases.ArrangeAsync" /> to arrange the request <see cref="HttpClient" />.
///   </para>
///   <para>
///     This pattern takes advantage of the
///     <a href="https://xunit.net/docs/shared-context#class-fixture">class fixture xUnit shared context API.</a>
///   </para>
/// </remarks>
public class ExampleComplexTest : BaseCaseAssertions<ExampleComplexTest.CaseFixtureWhichIsOnlyInitializedOnce>
{
  public ExampleComplexTest(CaseFixtureWhichIsOnlyInitializedOnce caseArrangementFixture)
    : base(caseArrangementFixture)
  {
  }

  [Fact]
  public void Sanity_AcquiredApiParameter()
  {
    Assert.NotEqual(Guid.Empty, CaseArrangement.ArrangedUserId);
  }

  [Fact]
  public void ApiReturnsExpectedValue()
  {
    var expected = CaseArrangement.ArrangedUserId.ToString("D");

    var actual = CaseArrangement.ActResult;

    Assert.Equal(expected, actual);
  }

  public class CaseFixtureWhichIsOnlyInitializedOnce : ICaseArrangementFixture
  {
    public Guid ArrangedUserId { get; private set; }

    public string ActResult { get; private set; } = string.Empty;

    public async Task ArrangeAsync()
    {
      ArrangedUserId = await SimulatedApiRequest();
      return;

      static async Task<Guid> SimulatedApiRequest()
      {
        await Task.Delay(1);
        return Guid.NewGuid();
      }
    }

    public async Task ActAsync()
    {
      ActResult = await SimulatedApiRequest(ArrangedUserId);
      return;

      static async Task<string> SimulatedApiRequest(Guid parameterRequiredByTheApi)
      {
        await Task.Delay(1);
        return parameterRequiredByTheApi.ToString("D");
      }
    }
  }
}
