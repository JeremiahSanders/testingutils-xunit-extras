namespace Jds.TestingUtils.Patterns;

/// <summary>
///     Methods which define a test case's pre-assert phases:
///     (1) arrange,
///     (2) acquire sanity values,
///     (3) act,
///     (4) acquire verification values.
/// </summary>
public interface ICasePhases
{
  /// <summary>
  ///     Executes test case arrangement steps (test case initialization phase 1 of 4).
  /// </summary>
  /// <remarks>
  ///     <para>This method should perform any initialization required to arrange the test case.</para>
  ///     <para>
  ///         Examples:
  ///         seed a database,
  ///         execute application- or test-specific &quot;initialization&quot; routines,
  ///         query a data source for test arrangement values.
  ///     </para>
  /// </remarks>
  Task ArrangeAsync();

  /// <summary>
  ///     Performs non-destructive steps to acquire &quot;sanity&quot; values which may be used by test assertions (test case
  ///     initialization phase 2 of 4).
  /// </summary>
  /// <remarks>
  ///     <para>
  ///         This method should perform calculations and queries to establish post-arrangement, pre-act conditions. Values
  ///         obtained during this test case initialization phase are useful for &quot;sanity&quot; tests: tests which make
  ///         assertions to ensure that assumptions or expectations are correct.
  ///     </para>
  ///     <para>
  ///         Examples:
  ///         query a database to verify that seeded data exists,
  ///         store an object property's value prior to an <see cref="ActAsync" /> expected to change it,
  ///         determine whether a file or directory exists before an <see cref="ActAsync" /> expected to create or update it.
  ///     </para>
  /// </remarks>
  Task AcquireSanityValuesAsync();

  /// <summary>
  ///     Performs the primary action(s) being tested (test case initialization phase 3 of 4).
  /// </summary>
  Task ActAsync();

  /// <summary>
  ///     Performs non-destructive steps to acquire post-act conditions (test case initialization phase 4 of 4).
  /// </summary>
  /// <remarks>
  ///     <para>
  ///         This method should perform calculations and queries to acquire post-act values which may be used by test
  ///         assertions. Values obtained during this test case initialization phase provide the primary basis for
  ///         &quot;side effect&quot; assertions.
  ///     </para>
  ///     <para>
  ///         Examples:
  ///         query a database to verify that expected data changed,
  ///         query a logging persistence abstraction for logged messages,
  ///         read a file which <see cref="ActAsync" /> was expected to have written.
  ///     </para>
  /// </remarks>
  Task AcquireVerificationValuesAsync();
}
