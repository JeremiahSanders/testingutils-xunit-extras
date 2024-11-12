using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jds.TestingUtils.Xunit2.Extras.Generators;

internal static class SharedContextCollectionGeneration
{
  internal static (string hintName, string source) CreateCollectionSource(ClassDeclarationSyntax classDeclaration)
  {
    var className = ClassDeclarationSyntaxParsing.GetClassName(classDeclaration);
    var namespaceName = ClassDeclarationSyntaxParsing.GetNamespaceName(classDeclaration);
    var source = CreateCollectionSource(namespaceName, className);
    return ($"{className}Collection", source);
  }

  internal static (string hintName, string source) CreateCollectionSource(RecordDeclarationSyntax recordDeclaration)
  {
    var recordName = RecordDeclarationSyntaxParsing.GetRecordName(recordDeclaration);
    var namespaceName = RecordDeclarationSyntaxParsing.GetNamespaceName(recordDeclaration);
    var source = CreateCollectionSource(namespaceName, recordName);
    return ($"{recordName}Collection", source);
  }

  internal static string CreateCollectionSource(string namespaceName, string className)
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
