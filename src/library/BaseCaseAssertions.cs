using Xunit;
using Xunit.Abstractions;

namespace Jds.TestingUtils.Xunit2.Extras;

/// <summary>
///     A base &quot;test class&quot; (i.e., a <c>class</c> which contains <c>xUnit</c>
///     <c>Fact</c> and <c>Theory</c> methods).
/// </summary>
/// <remarks>
///     <para>
///         xUnit will create a single instance of <typeparamref name="TCaseArrangementFixture"/> and provide the same instance
///         to each test method (due to <see cref="IClassFixture{TFixture}" />).
///     </para>
/// </remarks>
/// <typeparam name="TCaseArrangementFixture">A case arrangement fixture.</typeparam>
public abstract class BaseCaseAssertions<TCaseArrangementFixture> : IClassFixture<TCaseArrangementFixture>
    where TCaseArrangementFixture : class, ICaseArrangementFixture
{
    /// <summary>
    ///     Initializes a new instance of <see cref="BaseCaseAssertions{TCaseArrangementFixture}" />.
    /// </summary>
    /// <param name="caseArrangementFixture">
    ///     A case arrangement fixture instance.
    ///     It is expected that the fixture's <see cref="IAsyncLifetime.InitializeAsync" /> is complete.
    /// </param>
    /// <param name="testOutputHelper">A <see cref="ITestOutputHelper" />. Supports logging during assertion methods.</param>
    protected BaseCaseAssertions(TCaseArrangementFixture caseArrangementFixture, ITestOutputHelper testOutputHelper)
    {
        CaseArrangement = caseArrangementFixture;
        TestOutputHelper = testOutputHelper;
    }

    /// <summary>
    ///     Gets the case arrangement.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The <typeparamref name="TCaseArrangementFixture"/> is expected to expose properties which capture the results of
    ///         <see cref="ICasePhases.ActAsync" />, <see cref="ICasePhases.AcquireVerificationValuesAsync" />, and
    ///         <see cref="ICasePhases.AcquireSanityValuesAsync" />.
    ///     </para>
    /// </remarks>
    public TCaseArrangementFixture CaseArrangement { get; }

    /// <summary>
    ///     Gets the <see cref="ITestOutputHelper" /> provided by xUnit.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Derived types (test case assertion classes) test methods can use this to log messages.
    ///     </para>
    ///     <para>
    ///         This is most commonly used with sanity tests (tests making assertions about state after
    ///         <see cref="ICasePhases.ArrangeAsync" />, but before <see cref="ICasePhases.ActAsync" />).
    ///         In these tests it is used to log an arrangement's &quot;initial state&quot;.
    ///     </para>
    ///     <para>
    ///         When used in tests making assertions about the <see cref="ICasePhases.ActAsync" /> results, it is most
    ///         commonly used to log parameters and/or results.
    ///     </para>
    ///     <para>
    ///         When used in verification tests (tests making assertions about the values captured during
    ///         <see cref="ICasePhases.AcquireVerificationValuesAsync" />), it is most commonly used to log unexpected values
    ///         or failure context prior to invoking &quot;assert&quot; test methods (since they normally throw exceptions on
    ///         failures).
    ///     </para>
    /// </remarks>
    public ITestOutputHelper TestOutputHelper { get; }
}
