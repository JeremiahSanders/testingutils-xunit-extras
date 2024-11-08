using FluentAssertions;

namespace Jds.TestingUtils.Xunit2.Extras.Tests.Unit;

public static class CaseArrangementFixtureTests
{
  public class DoesNotNeedMethodImplementations : ICaseArrangementFixture
  {
    [Fact]
    public void InterfaceProvidesImplementations()
    {
      true.Should().BeTrue();
    }
  }

  public class ImplementingMethodsAsPublicWorksFine : ICaseArrangementFixture
  {
    private bool _acquireSanityValuesExecuted;
    private bool _acquireVerificationValuesExecuted;
    private bool _actExecuted;
    private bool _arrangeExecuted;

    public Task ArrangeAsync()
    {
      _arrangeExecuted = true;
      return Task.CompletedTask;
    }

    public Task ActAsync()
    {
      _actExecuted = true;
      return Task.CompletedTask;
    }

    public Task AcquireSanityValuesAsync()
    {
      _acquireSanityValuesExecuted = true;
      return Task.CompletedTask;
    }

    public Task AcquireVerificationValuesAsync()
    {
      _acquireVerificationValuesExecuted = true;
      return Task.CompletedTask;
    }

    [Fact]
    public void ArrangeExecuted()
    {
      _arrangeExecuted.Should().BeTrue();
    }

    [Fact]
    public void ActExecuted()
    {
      _actExecuted.Should().BeTrue();
    }

    [Fact]
    public void AcquireSanityValuesExecuted()
    {
      _acquireSanityValuesExecuted.Should().BeTrue();
    }

    [Fact]
    public void AcquireVerificationValuesExecuted()
    {
      _acquireVerificationValuesExecuted.Should().BeTrue();
    }
  }

  public class ImplementingMethodsAsInternalDoesNotWork : ICaseArrangementFixture
  {
    private bool _acquireSanityValuesExecuted;
    private bool _acquireVerificationValuesExecuted;
    private bool _actExecuted;
    private bool _arrangeExecuted;

    internal Task ArrangeAsync()
    {
      _arrangeExecuted = true;
      return Task.CompletedTask;
    }

    internal Task ActAsync()
    {
      _actExecuted = true;
      return Task.CompletedTask;
    }

    internal Task AcquireSanityValuesAsync()
    {
      _acquireSanityValuesExecuted = true;
      return Task.CompletedTask;
    }

    internal Task AcquireVerificationValuesAsync()
    {
      _acquireVerificationValuesExecuted = true;
      return Task.CompletedTask;
    }

    [Fact]
    public void ArrangeNotExecuted()
    {
      _arrangeExecuted.Should().BeFalse();
    }

    [Fact]
    public void ActNotExecuted()
    {
      _actExecuted.Should().BeFalse();
    }

    [Fact]
    public void AcquireSanityValuesNotExecuted()
    {
      _acquireSanityValuesExecuted.Should().BeFalse();
    }

    [Fact]
    public void AcquireVerificationValuesNotExecuted()
    {
      _acquireVerificationValuesExecuted.Should().BeFalse();
    }
  }

  public class ImplementingMethodsAsPrivateDoesNotWork : ICaseArrangementFixture
  {
    private bool _acquireSanityValuesExecuted;
    private bool _acquireVerificationValuesExecuted;
    private bool _actExecuted;
    private bool _arrangeExecuted;

    private Task ArrangeAsync()
    {
      _arrangeExecuted = true;
      return Task.CompletedTask;
    }

    private Task ActAsync()
    {
      _actExecuted = true;
      return Task.CompletedTask;
    }

    private Task AcquireSanityValuesAsync()
    {
      _acquireSanityValuesExecuted = true;
      return Task.CompletedTask;
    }

    private Task AcquireVerificationValuesAsync()
    {
      _acquireVerificationValuesExecuted = true;
      return Task.CompletedTask;
    }

    [Fact]
    public void ArrangeNotExecuted()
    {
      _arrangeExecuted.Should().BeFalse();
    }

    [Fact]
    public void ActNotExecuted()
    {
      _actExecuted.Should().BeFalse();
    }

    [Fact]
    public void AcquireSanityValuesNotExecuted()
    {
      _acquireSanityValuesExecuted.Should().BeFalse();
    }

    [Fact]
    public void AcquireVerificationValuesNotExecuted()
    {
      _acquireVerificationValuesExecuted.Should().BeFalse();
    }
  }

  public class ImplementingMethodsUsingInterfaceMethodsWorksFine : ICaseArrangementFixture
  {
    private bool _acquireSanityValuesExecuted;
    private bool _acquireVerificationValuesExecuted;
    private bool _actExecuted;
    private bool _arrangeExecuted;

    Task ICasePhases.ArrangeAsync()
    {
      _arrangeExecuted = true;
      return Task.CompletedTask;
    }

    Task ICasePhases.ActAsync()
    {
      _actExecuted = true;
      return Task.CompletedTask;
    }

    Task ICasePhases.AcquireSanityValuesAsync()
    {
      _acquireSanityValuesExecuted = true;
      return Task.CompletedTask;
    }

    Task ICasePhases.AcquireVerificationValuesAsync()
    {
      _acquireVerificationValuesExecuted = true;
      return Task.CompletedTask;
    }

    [Fact]
    public void ArrangeExecuted()
    {
      _arrangeExecuted.Should().BeTrue();
    }

    [Fact]
    public void ActExecuted()
    {
      _actExecuted.Should().BeTrue();
    }

    [Fact]
    public void AcquireSanityValuesExecuted()
    {
      _acquireSanityValuesExecuted.Should().BeTrue();
    }

    [Fact]
    public void AcquireVerificationValuesExecuted()
    {
      _acquireVerificationValuesExecuted.Should().BeTrue();
    }
  }
}
