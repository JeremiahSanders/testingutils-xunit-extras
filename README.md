# TestingUtils.Xunit.Extras ([NuGet][]) ([GitHub][])

This library supplements `xUnit`. Specifically, it adds pattern `interface`s, structured test `class`es and source generators.

## How To Use

Add the [NuGet package][NuGet] to your xUnit test project. (Currently only xUnit 2 is supported.)

### Test Class with Structured Execution/Initialization Steps

> **In this scenario the execution/initialization steps occur once for _each_ test (assertion) method.**

In this scenario, the goal is to add one or more structured steps to a test (assertion) class. These steps are expected to be completed _before **any**_ of the test (assertion) methods. Additionally, the steps _might_ require `async` work.

#### To Implement Test Class with Structured Execution/Initialization Steps

Create a normal xUnit test class and **either** _inherit from_ `Jds.TestingUtils.Xunit2.Extras.BaseCaseFixture` **or**, if you need to inherit from another type, implement `Jds.TestingUtils.Xunit2.Extras.ICaseArrangementFixture`.

When inheriting from `BaseCaseFixture` there are `protected` methods to `override` (`ArrangeAsync`, `ActAsync`, etc.). Override any (or none) to support your needed arrangement.

When implementing `ICaseArrangementFixture` all structured test case phases (`ICasePhases`) have a default implementation provided by the interface. Implement any or all of the following: `ICasePhases.ArrangeAsync`, `ICasePhases.AcquireSanityValuesAsync`, `ICasePhases.ActAsync`, `ICasePhases.AcquireVerificationValuesAsync`. If the test requires any cleanup (e.g., deleting temporary database resources, files), implement `IDestructiveCase.CleanupAsync`.

See [example `ExampleTestClassWithRepeatedInitialization` implementation][ExampleTestClassWithRepeatedInitialization].

### Test Class with Single-Invocation Execution/Initialization

> **In this scenario the execution/initialization steps _only_ occur _once_**, irrespective of how many test (assertion) methods are invoked against it.

In this scenario, the goal is to add one or more structured steps to a test **case fixture** `class`. These steps are expected to be completed _before **any**_ of the test (assertion) methods. Additionally, the steps _might_ require `async` work.

The basic idea in this scenario is that the "case fixture" is created by xUnit. As usual the "case assertion" class (test class) is constructed once for each test (assertion) method. However, in this scenario the case assertion class receives the case fixture from xUnit during construction. The _same_ fixture class _instance_ is used when constructing the test class for _each_ assertion method.

> This pattern is useful for ASP.NET Core integration tests and other scenarios where you need to perform some work (such as an `async` web API request) and make _multiple_ assertions (within distinct test methods) upon the same output, e.g., assert upon the `HttpStatusCode` separately from headers' content and response body.

#### To Implement Test Class with Single-Invocation Execution/Initialization

Create a "case fixture" class which inherits from `Jds.TestingUtils.Xunit2.Extras.BaseCaseFixture`. Create a test "assertion" class which inherits from `Jds.TestingUtils.Xunit2.Extras.BaseCaseAssertions<TCaseArrangementFixture>` (where `TCaseArrangementFixture` is your **case fixture** class which inherited `BaseCaseFixture`).

When inheriting from `BaseCaseFixture` there are `protected` methods to `override` (`ArrangeAsync`, `ActAsync`, etc.). Override any (or none) to support your needed arrangement.

Add assertions (i.e., `[Fact]` and `[Theory]` methods) to the "assertion" class.

See [example `ExampleComplexTest` implementation][ExampleComplexTest].

### Shared Case Context/Configuration across Multiple Test Cases

In this scenario, the goal is to create a "context" value object (e.g., to contain shared configuration) which will be passed to _multiple_ test **case fixture** `class`es, facilitating coordinated tests.

> **Important:** This pattern assumes that your "context" is essentially **read-only** after construction. It is **not advised** to _mutate_ or _add_ data in the "context" to avoid unexpected results.

The basic idea in this scenario is that you design a **context** value object to hold the shared configuration. During test execution xUnit will construct an instance and pass it to all the associated **case fixture** classes. Each case fixture is intended to perform a single logical test "case" arrangement, which will occur _prior to all_ test assertions. The _same instance_ of the **context** is passed to _every_ **case fixture**. Further, the same **case fixture** instance is passed to every related "assertion" class.

> _Without this library_, this is a complicated situation to arrange, due to the limits of xUnit test class dependency injection. Implementation normally requires creating an xUnit `ICollectionFixture` (including `[CollectionDefinition]` attribute), a base case fixture class with reliable constructor signature, and a base test case assertion class which both uses `IClassFixture<>` and has a compatible constructor signature—all while placing `[Collection]` attributes on the correct classes to enable xUnit dependency injection.
>
> This pattern is **particularly** useful in ASP.NET Core integration test projects. It enables things like establishing a "context" object with shared application configuration (e.g., Entity Framework connection strings, validation options) and consistently using those values which are expected to be stable for the life of the "context". The authors have frequently used this pattern with generated data to enable verifying that side effects (like logging or external API requests) are invoked using values from the application's `IConfiguration`.

#### To Implement Shared Case Context/Configuration across Multiple Test Cases

Create a **shared case context** `class` with a **parameterless constructor**. (Parameterless constructor is very important for xUnit.) Apply the `[Jds.TestingUtils.Xunit2.Extras.SharedCaseContext]` attribute to the class. This will trigger source generation (during compile), which creates _new classes_ in _your assembly_ which use your shared case context. Specifically, it creates a `{YourContextName}Fixture` base fixture class, a `{YourContextName}Assertions` class, and the additional classes and configuration needed to support xUnit dependency injection.

After the shared case context is created:

To create a "Test Class with Structured Execution/Initialization Steps" (see above) which uses the shared case context, just create a new test fixture which inherits from `{YourContextName}Fixture`. Access the shared context object via its `.Context` property.

To create a "Test Class with Single-Invocation Execution/Initialization" (see above) which uses the shared case context, create a new test case fixture which inherits from `{YourContextName}Fixture` and implement any test case phase methods needed. Then, create a test assertion class which inherits from `{YourContextName}Assertions<TCaseFixture>`, where `TCaseFixture` is the class you just created which inherited `{YourContextName}Fixture`.

See [example `SharedContextExample` implementation][SharedContextExample].

## API Documentation

* [`Jds.TestingUtils.Xunit2.Extras`][] namespace

[`Jds.TestingUtils.Xunit2.Extras`]: https://github.com/JeremiahSanders/testingutils-xunit-extras/tree/main/docs/api/TestingUtils.Xunit2.Extras.md
[ExampleComplexTest]: https://github.com/JeremiahSanders/testingutils-xunit-extras/blob/dev/tests/unit/Examples/WithExtras/SingleInitialization/ExampleComplexTest.cs
[ExampleTestClassWithRepeatedInitialization]: https://github.com/JeremiahSanders/testingutils-xunit-extras/tree/dev/tests/unit/Examples/WithExtras/RepeatedInitialization/ExampleTestClassWithRepeatedInitialization.cs
[GitHub]: https://github.com/JeremiahSanders/testingutils-xunit-extras/
[NuGet]: https://www.nuget.org/packages/Jds.TestingUtils.Xunit2.Extras/
[SharedContextExample]: https://github.com/JeremiahSanders/testingutils-xunit-extras/blob/dev/tests/unit/Examples/WithExtras/SharedConfiguration/SharedContextExample.cs