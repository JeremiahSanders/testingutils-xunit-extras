using Jds.TestingUtils.Patterns;
using Xunit;

namespace Jds.TestingUtils.Xunit2.Extras;

/// <summary>
///   A general-purpose test case fixture.
/// </summary>
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
