namespace Jds.TestingUtils.Xunit2.Extras;

/// <summary>
///   Attribute which declares that this <c>class</c> (the <c>class</c> to which it is applied) is designed to be a
///   &quot;shared case context.&quot;
///   Applying this attribute triggers the generation of an <c>abstract</c> base test case fixture <c>class</c>, via
///   <see cref="SharedCaseContextGenerator" />.
///   The generated test case fixture <c>class</c> will be named to match this class, with a &quot;Fixture&quot; suffix.
///   For example, if this class is named <c>ExampleSharedContext</c>, then the generated <c>class</c> would be named
///   <c>ExampleSharedContextFixture</c>.
/// </summary>
/// <remarks>
///   <para>
///     A shared case context is understood to be a lightweight object which exposes <c>readonly</c>, immutable properties.
///   </para>
///   <para>
///     The properties exposed by a shared case context are generally common configuration or &quot;seed data&quot; values.
///   </para>
/// </remarks>
[AttributeUsage(AttributeTargets.Class)]
public class SharedCaseContextAttribute : Attribute
{
  // public SharedCaseContextAttribute() : this(true)
  // {
  // }
  //
  // public SharedCaseContextAttribute(bool useStructuredExecution)
  // {
  //   UseStructuredExecution = useStructuredExecution;
  // }
  //
  // /// <summary>
  // ///   Gets a value indicating whether the generated shared case context fixture implements
  // ///   <see cref="ICaseArrangementFixture" />, which provides a comprehensive test fixture case execution workflow.
  // /// </summary>
  // public bool UseStructuredExecution { get; }
}
