namespace Jds.TestingUtils.Xunit2.Extras;

/// <summary>
///   Methods which provide an API to clean up changes made by the test case arrangement.
/// </summary>
/// <remarks>
///   <para>
///     Examples:
///     remove database records seeded during &quot;arrange&quot;,
///     deleting files created by &quot;act&quot;.
///   </para>
/// </remarks>
public interface IDestructiveCase
{
  /// <summary>
  ///   Performs cleanup of any resources created by the test case.
  /// </summary>
  /// <remarks>
  ///   <para>Conceptually, this occurs after all test initialization and assertions are complete.</para>
  ///   <para>Use this method to undo changes which might impact other tests or the executing environment.</para>
  ///   <para>
  ///     Usage examples:
  ///     remove database records seeded during &quot;arrange&quot; (reducing unexpected collisions with other tests),
  ///     deleting files created by &quot;act&quot; (saving storage space),
  ///     delete or revoke temporary permissions/authorization specific to this test case (reducing security exposure).
  ///   </para>
  /// </remarks>
  Task CleanupAsync();
}
