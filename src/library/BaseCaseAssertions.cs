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
    protected BaseCaseAssertions(TCaseArrangementFixture caseArrangementFixture)
    {
        CaseArrangement = caseArrangementFixture;
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
    protected TCaseArrangementFixture CaseArrangement { get; }
}
