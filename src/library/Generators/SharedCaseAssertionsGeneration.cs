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
    [Xunit.Collection(""{className}"")]
    public abstract class {className}Assertions<TCaseArrangementFixture> : Jds.TestingUtils.Xunit2.Extras.BaseCaseAssertions<TCaseArrangementFixture>
      where TCaseArrangementFixture : {className}Fixture
    {{
        protected {className}Assertions(TCaseArrangementFixture fixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
          : base(fixture, testOutputHelper)
        {{
        }}
    }}
}}");
      return builder.ToString();
    }
  }
}
