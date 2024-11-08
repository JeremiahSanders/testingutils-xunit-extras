using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jds.TestingUtils.Xunit2.Extras.Generators;

internal static class SharedCaseFixtureGeneration
{
  public static (string hintName, string source) CreateFixtureSource(ClassDeclarationSyntax classDeclaration)
  {
    var className = ClassDeclarationSyntaxParsing.GetClassName(classDeclaration);
    var namespaceName = ClassDeclarationSyntaxParsing.GetNamespaceName(classDeclaration);
    var source = BuildSource(namespaceName, className);
    return ($"{className}Fixture", source);

    static string BuildSource(string namespaceName, string className)
    {
      var builder = new StringBuilder($@"
using JdsCasePhases = Jds.TestingUtils.Xunit2.Extras.ICasePhases;
using JdsDestructiveCase = Jds.TestingUtils.Xunit2.Extras.IDestructiveCase;
using JdsCaseArrangementFixture = Jds.TestingUtils.Xunit2.Extras.BaseCaseFixture;

namespace {namespaceName}
{{
    /// <summary>A base test case arrangement fixture which has access to the shared <see cref=""{className}"" /> context object.</summary>
    /// <remarks>
    ///   <para>How to use:</para>
    ///   <para>
    ///     Override the <see cref=""ArrangeAsync"" />, <see cref=""AcquireSanityValuesAsync"" />,
    ///     <see cref=""ActAsync"" />, <see cref=""AcquireVerificationValuesAsync"" />,
    ///     and <see cref=""CleanupAsync"" /> methods as needed.
    ///     Then, create an &quot;assertion/test&quot; class, based on <see cref=""{className}Assertions{{TCaseArrangementFixture}}"" />,
    ///     where <c>TCaseArrangementFixture</c> is this class.
    ///   </para>
    ///   <para>
    ///     When xUnit constructs this class it will invoke the test phase methods once for this fixture.
    ///     <see cref=""ArrangeAsync"" />, <see cref=""AcquireSanityValuesAsync"" />,
    ///     <see cref=""ActAsync"" />, and <see cref=""AcquireVerificationValuesAsync"" />
    ///     execute before the test methods in the &quot;assertions&quot; class (which will perform assertions
    ///     related to this test case arrangement).
    ///   </para>
    ///   <para>
    ///     <see cref=""CleanupAsync"" /> is invoked by xUnit during asynchronous disposal of this class.
    ///   </para>
    ///   <para>
    ///     IMPORTANT: xUnit has limited dependency injection support. Make sure that your non-abstract test fixtures
    ///     require no constructor parameters other than <see cref=""{className}"" />.
    ///   </para>
    /// </remarks>
    public class {className}Fixture : JdsCaseArrangementFixture
    {{
        /// <summary>Gets the shared <see cref=""{className}"" /> object.</summary>
        /// <remarks>This context is expected to contain shared, <c>readonly</c> configuration and dependencies for this fixture.</remarks>
        public {className} Context {{ get; init; }}

        public {className}Fixture({className} context)
        {{
            Context = context;
        }}
    }}
}}");
      return builder.ToString();
    }
  }
}
