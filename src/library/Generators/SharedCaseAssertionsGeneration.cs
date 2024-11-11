using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jds.TestingUtils.Xunit2.Extras.Generators;

internal static class SharedCaseAssertionsGeneration
{
  public static (string hintName, string source) CreateAssertionsSource(ClassDeclarationSyntax classDeclaration)
  {
    var className = ClassDeclarationSyntaxParsing.GetClassName(classDeclaration);
    var namespaceName = ClassDeclarationSyntaxParsing.GetNamespaceName(classDeclaration);
    var source = BuildSource(namespaceName, className);
    return ($"{className}Assertions", source);

    static string BuildSource(string namespaceName, string className)
    {
      var builder = new StringBuilder($@"
namespace {namespaceName}
{{
    /// <summary>
    ///   A base test class which makes assertions related to <typeparamref name=""TCaseArrangementFixture""/> and
    ///   has access to a shared <see cref=""{className}""/> context object.
    /// </summary>
    [Xunit.Collection(""{className}"")]
    public abstract class {className}Assertions<TCaseArrangementFixture> : Jds.TestingUtils.Xunit2.Extras.BaseCaseAssertions<TCaseArrangementFixture>
      where TCaseArrangementFixture : {className}Fixture
    {{
        protected {className}Assertions(TCaseArrangementFixture fixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
          : base(fixture, testOutputHelper)
        {{
        }}

        /// <summary>Gets the shared case context object exposed by <see cref=""TCaseArrangementFixture""/></summary>
        protected {className} CaseContext => CaseArrangement.Context;
    }}

    /// <summary>A base test class which has access to a shared <see cref=""{className}""/> context object.</summary>
    public abstract class {className}Assertions : {className}Assertions<{className}Fixture>
    {{
        protected {className}Assertions({className}Fixture fixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
          : base(fixture, testOutputHelper)
        {{
        }}
    }}
}}");
      return builder.ToString();
    }
  }
}
