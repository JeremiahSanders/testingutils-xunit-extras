using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace Jds.TestingUtils.Xunit2.Extras.Generators;

/// <summary>
///   A source generator which produces a base test case arrangement fixture,
///   a base &quot;assertion&quot;/&quot;test&quot; class,
///   and an implementation of <see cref="ICollectionFixture{TFixture}" /> (required for xUnit).
/// </summary>
[Generator]
public class SharedCaseContextGenerator : ISourceGenerator
{
  /// <inheritdoc />
  public void Initialize(GeneratorInitializationContext context)
  {
    context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
  }

  /// <inheritdoc />
  public void Execute(GeneratorExecutionContext context)
  {
    if (context.SyntaxReceiver is SyntaxReceiver receiver)
    {
      foreach (var recordDeclaration in receiver.CandidateRecords.Distinct())
      {
        var (collectionHintName, collectionSource) =
          SharedContextCollectionGeneration.CreateCollectionSource(recordDeclaration);
        context.AddSource(collectionHintName, collectionSource);

        var (assertionsHintName, assertionsSource) =
          SharedCaseAssertionsGeneration.CreateAssertionsSource(recordDeclaration);
        context.AddSource(assertionsHintName, assertionsSource);

        var (fixtureHintName, fixtureSource) = SharedCaseFixtureGeneration.CreateFixtureSource(recordDeclaration);
        context.AddSource(fixtureHintName, fixtureSource);
      }

      foreach (var classDeclaration in receiver.CandidateClasses.Distinct())
      {
        var (collectionHintName, collectionSource) =
          SharedContextCollectionGeneration.CreateCollectionSource(classDeclaration);
        context.AddSource(collectionHintName, collectionSource);

        var (assertionsHintName, assertionsSource) =
          SharedCaseAssertionsGeneration.CreateAssertionsSource(classDeclaration);
        context.AddSource(assertionsHintName, assertionsSource);

        var (fixtureHintName, fixtureSource) = SharedCaseFixtureGeneration.CreateFixtureSource(classDeclaration);
        context.AddSource(fixtureHintName, fixtureSource);
      }
    }
  }


  private class SyntaxReceiver : ISyntaxReceiver
  {
    public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();
    public List<RecordDeclarationSyntax> CandidateRecords { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
      const string shortName = "SharedCaseContext";
      const string attributeName = "SharedCaseContextAttribute";
      if (syntaxNode is RecordDeclarationSyntax { AttributeLists.Count: > 0 } recordDeclarationSyntax)
      {
        foreach (var attributeList in recordDeclarationSyntax.AttributeLists)
        {
          foreach (var attribute in attributeList.Attributes)
          {
            if (attribute.Name.ToString() is shortName or attributeName)
            {
              CandidateRecords.Add(recordDeclarationSyntax);
              break;
            }
          }
        }
      }
      else if (syntaxNode is ClassDeclarationSyntax { AttributeLists.Count: > 0 } classDeclarationSyntax)
      {
        foreach (var attributeList in classDeclarationSyntax.AttributeLists)
        {
          foreach (var attribute in attributeList.Attributes)
          {
            if (attribute.Name.ToString() is shortName or attributeName)
            {
              CandidateClasses.Add(classDeclarationSyntax);
              break;
            }
          }
        }
      }
    }
  }
}
