using Xunit;

namespace Jds.TestingUtils.Xunit2.Extras;

/// <summary>
///   A general-purpose test case fixture.
///   Apply to an xUnit &quot;test class&quot; (i.e., a class which contains one or more facts (
///   <see cref="FactAttribute" />) or theories (<see cref="TheoryAttribute" />)).
/// </summary>
/// <remarks>
///   <para>
///     Implementing this interface creates default implementations of all <see cref="ICasePhases" /> (e.g.,
///     <see cref="ICasePhases.ActAsync" />).
///   </para>
///   <para>
///     Additionally, this interface implements xUnit's <see cref="IAsyncLifetime" />.
///     It executes <see cref="ICasePhases.ArrangeAsync" />, <see cref="ICasePhases.AcquireSanityValuesAsync" />,
///     <see cref="ICasePhases.ActAsync" />, and <see cref="ICasePhases.AcquireVerificationValuesAsync" /> during
///     <see cref="IAsyncLifetime.InitializeAsync" />.
///     It executes <see cref="IDestructiveCase.CleanupAsync" /> during <see cref="IAsyncLifetime.DisposeAsync" />.
///   </para>
///   <para>
///     How to use: implement one or more of the <see cref="ICasePhases" /> methods.
///   </para>
/// </remarks>
public interface ICaseArrangementFixture : IAsyncLifetime, ICasePhases, IDestructiveCase
{
  /// <inheritdoc />
  async Task IAsyncLifetime.InitializeAsync()
  {
    await ArrangeAsync();
    await AcquireSanityValuesAsync();
    await ActAsync();
    await AcquireVerificationValuesAsync();
  }

  /// <inheritdoc />
  async Task IAsyncLifetime.DisposeAsync()
  {
    await CleanupAsync();
  }

  /// <inheritdoc />
  Task ICasePhases.ArrangeAsync()
  {
    return Task.CompletedTask;
  }

  /// <inheritdoc />
  Task ICasePhases.AcquireSanityValuesAsync()
  {
    return Task.CompletedTask;
  }

  /// <inheritdoc />
  Task ICasePhases.AcquireVerificationValuesAsync()
  {
    return Task.CompletedTask;
  }

  /// <inheritdoc />
  Task IDestructiveCase.CleanupAsync()
  {
    return Task.CompletedTask;
  }
}
