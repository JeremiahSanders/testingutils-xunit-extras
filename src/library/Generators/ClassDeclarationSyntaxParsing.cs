using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jds.TestingUtils.Xunit2.Extras.Generators;

internal static class ClassDeclarationSyntaxParsing
{
  internal static string GetClassName(ClassDeclarationSyntax classDeclaration)
  {
    return classDeclaration.Identifier.ToString();
  }

  internal static string GetNamespaceName(ClassDeclarationSyntax classDeclaration)
  {
    SyntaxNode? currentNode = classDeclaration;
    string? namespaceName = null;

    // Traverse to the root to find the namespace or file-scoped namespace
    while (currentNode != null)
    {
      namespaceName = currentNode switch
      {
        FileScopedNamespaceDeclarationSyntax namespaceDeclarationSyntax => namespaceDeclarationSyntax.Name
          .ToString(),
        NamespaceDeclarationSyntax namespaceDeclaration => namespaceDeclaration.Name.ToString(),
        CompilationUnitSyntax { Usings.Count: > 0 } compilationUnit => compilationUnit.Usings[0]
          .Name?.ToString(),
        _ => namespaceName
      };

      if (namespaceName != null) break;

      currentNode = currentNode.Parent;
    }

    namespaceName ??= "FallbackNamespace";
    return namespaceName;
  }
}
