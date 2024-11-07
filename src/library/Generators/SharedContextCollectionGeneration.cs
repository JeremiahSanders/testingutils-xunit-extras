using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jds.TestingUtils.Xunit2.Extras.Generators;

internal static class SharedContextCollectionGeneration
{
  public static (string hintName, string source) CreateCollectionSource(ClassDeclarationSyntax classDeclaration)
  {
    var className = ClassDeclarationSyntaxParsing.GetClassName(classDeclaration);
    var namespaceName = ClassDeclarationSyntaxParsing.GetNamespaceName(classDeclaration);
    var source = BuildSource(namespaceName, className);
    return ($"{className}Collection", source);

    static string BuildSource(string namespaceName, string className)
    {
      var builder = new StringBuilder($@"
namespace {namespaceName}
{{
    [Xunit.CollectionDefinition(""{className}"")]
    public class {className}Collection : Xunit.ICollectionFixture<{className}>
    {{
    }}
}}");
      return builder.ToString();
    }
  }
}
