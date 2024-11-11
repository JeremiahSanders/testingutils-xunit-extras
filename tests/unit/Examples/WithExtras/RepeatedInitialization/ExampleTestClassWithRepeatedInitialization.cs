using System.Diagnostics.CodeAnalysis;
using Xunit.Abstractions;

namespace Jds.TestingUtils.Xunit2.Extras.Tests.Unit.Examples.WithExtras.RepeatedInitialization;

/// <summary>
///   This test class shows a simple use of <see cref="ICaseArrangementFixture" /> to provide test structure.
/// </summary>
/// <remarks>
///   <para>
///     xUnit invokes every test method in this class using a distinct instance. As a result, each <c>Fact</c> has
///     different arrangement values (since we randomly generate them in the constructor).
///   </para>
///   <para>
///     When writing tests following this pattern (by implementing <see cref="ICaseArrangementFixture" /> on your
///     test class), remember that the whole arrangement workflow will be repeated for every test method.
///   </para>
/// </remarks>
public class ExampleTestClassWithRepeatedInitialization : ICaseArrangementFixture
{
  private readonly int _exampleIntAboveZeroArrangementValue;
  private readonly int _exampleIntBelowZeroArrangementValue;
  private readonly string _exampleStringArrangementValue;
  private readonly ITestOutputHelper _testOutputHelper;

  public ExampleTestClassWithRepeatedInitialization(ITestOutputHelper testOutputHelper)
  {
    _testOutputHelper = testOutputHelper;
    _exampleIntAboveZeroArrangementValue = Random.Shared.Next(1, 20);
    _exampleIntBelowZeroArrangementValue = Random.Shared.Next(-100, 0);
    _exampleStringArrangementValue = Guid.NewGuid().ToString();
  }

  public string? ActResultIntGreaterThanZero { get; private set; }

  public string? ActResultIntLessThanZero { get; set; }

  public Exception? ActExceptionIntLessThanZero { get; private set; }

  public Exception? ActExceptionIntGreaterThanZero { get; private set; }

  [SuppressMessage("Usage", "xUnit1013:Public method should be marked as test")]
  public async Task ActAsync()
  {
    try
    {
      ActResultIntGreaterThanZero =
        await ExampleMethodUnderTest(_exampleIntAboveZeroArrangementValue, _exampleStringArrangementValue);
    }
    catch (Exception exception)
    {
      ActExceptionIntGreaterThanZero = exception;
      _testOutputHelper.WriteLine("Exception ({1}) occurred during Act: {0}", exception.Message,
        exception.GetType().Name);
    }

    try
    {
      ActResultIntLessThanZero =
        await ExampleMethodUnderTest(_exampleIntBelowZeroArrangementValue, _exampleStringArrangementValue);
    }
    catch (Exception exception)
    {
      ActExceptionIntLessThanZero = exception;
      // Not logging here because we expect this to result in an exception.
    }
  }

  [Fact]
  public void GivenValueAboveZeroReturnsExpectedValue()
  {
    var expected = _exampleStringArrangementValue + _exampleIntAboveZeroArrangementValue;

    Assert.Equal(expected, ActResultIntGreaterThanZero);
    Assert.Null(ActExceptionIntGreaterThanZero);
  }

  [Fact]
  public void GivenValueBelowZeroFails()
  {
    Assert.Null(ActResultIntLessThanZero);
    Assert.IsType<ArgumentOutOfRangeException>(ActExceptionIntLessThanZero);
  }

  /// <summary>
  ///   An example representing a method under test.
  /// </summary>
  public static Task<string> ExampleMethodUnderTest(int intParam, string stringParam)
  {
    return intParam < 0
      ? Task.FromException<string>(new ArgumentOutOfRangeException(nameof(intParam), intParam,
        "Value must be greater than or equal to 0"))
      : Task.FromResult($"{stringParam}{intParam}");
  }
}
