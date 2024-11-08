using Xunit;

namespace Jds.TestingUtils.Xunit2.Extras;

/// <summary>
///   A base test case arrangement fixture which has default implementations for all <see cref="ICasePhases" /> and
///   <see cref="IDestructiveCase" /> methods and adapts them to run in <see cref="IAsyncLifetime" />.
/// </summary>
/// <remarks>
///   <para>Inherit from this class to easily apply structured test execution to a test fixture.</para>
///   <para>When inheriting from this class:</para>
///   <para>
///     If the class contains any xUnit <c>Fact</c> or <c>Theory</c> methods, then it will be executed like
///     an (assertion) test class. <see cref="ArrangeAsync" />, <see cref="AcquireSanityValuesAsync" />,
///     <see cref="ActAsync" />, and <see cref="AcquireVerificationValuesAsync" /> will each
///     execute before every test method.
///   </para>
///   <para>
///     If the class does NOT contain any xUnit <c>Fact</c> or <c>Theory</c> methods,
///     then the class can be an xUnit &quot;class fixture&quot;. To do so, create a test assertion class based on
///     <see cref="BaseCaseAssertions{TCaseArrangementFixture}" /> where its <c>TCaseArrangementFixture</c> is
///     the fixture class derived from this type. xUnit will create one instance of the derived fixture and use it
///     for each test method. The fixture's methods will only be called ONCE, for the whole test assertion class.
///   </para>
/// </remarks>
public class BaseCaseFixture : ICaseArrangementFixture
{
  /// <inheritdoc />
  Task ICasePhases.ActAsync()
  {
    return ActAsync();
  }

  /// <inheritdoc />
  Task ICasePhases.ArrangeAsync()
  {
    return ArrangeAsync();
  }

  /// <inheritdoc />
  Task ICasePhases.AcquireSanityValuesAsync()
  {
    return AcquireSanityValuesAsync();
  }

  /// <inheritdoc />
  Task ICasePhases.AcquireVerificationValuesAsync()
  {
    return AcquireVerificationValuesAsync();
  }

  /// <inheritdoc />
  Task IDestructiveCase.CleanupAsync()
  {
    return CleanupAsync();
  }

  /// <inheritdoc cref="Jds.TestingUtils.Xunit2.Extras.ICasePhases.ArrangeAsync" />
  protected virtual Task ArrangeAsync()
  {
    return Task.CompletedTask;
  }

  /// <inheritdoc cref="Jds.TestingUtils.Xunit2.Extras.ICasePhases.AcquireSanityValuesAsync" />
  protected virtual Task AcquireSanityValuesAsync()
  {
    return Task.CompletedTask;
  }

  /// <inheritdoc cref="Jds.TestingUtils.Xunit2.Extras.ICasePhases.ActAsync" />
  protected virtual Task ActAsync()
  {
    return Task.CompletedTask;
  }

  /// <inheritdoc cref="Jds.TestingUtils.Xunit2.Extras.ICasePhases.AcquireVerificationValuesAsync" />
  protected virtual Task AcquireVerificationValuesAsync()
  {
    return Task.CompletedTask;
  }

  /// <inheritdoc cref="Jds.TestingUtils.Xunit2.Extras.IDestructiveCase.CleanupAsync" />
  protected virtual Task CleanupAsync()
  {
    return Task.CompletedTask;
  }
}
