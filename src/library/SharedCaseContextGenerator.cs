using System.Text;
using Jds.TestingUtils.Xunit2.Extras.GeneratorInternal;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace Jds.TestingUtils.Xunit2.Extras;

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
      foreach (var classDeclaration in receiver.CandidateClasses.Distinct())
      {
        var (hintName, source) = CreateSource(classDeclaration);
        context.AddSource(hintName, source);
      }
  }

  internal static (string hintName, string source) CreateSource(ClassDeclarationSyntax classDeclaration)
  {
    var className = ClassDeclarationSyntaxParsing.GetClassName(classDeclaration);
    var namespaceName = ClassDeclarationSyntaxParsing.GetNamespaceName(classDeclaration);
    var source = BuildSource(namespaceName, className);
    return ($"{className}Collection", source);

    static string BuildSource(string namespaceName, string className)
    {
      var builder = new StringBuilder($@"
using XunitCollectionDefinition = Xunit.CollectionDefinitionAttribute;
using XunitCollection = Xunit.CollectionAttribute;
using XunitTestOutputHelper = Xunit.Abstractions.ITestOutputHelper;
using JdsCasePhases = Jds.TestingUtils.Xunit2.Extras.ICasePhases;
using JdsDestructiveCase = Jds.TestingUtils.Xunit2.Extras.IDestructiveCase;
using JdsCaseArrangementFixture = Jds.TestingUtils.Xunit2.Extras.ICaseArrangementFixture;

namespace {namespaceName}
{{
    [XunitCollectionDefinition(""{className}"")]
    public class {className}Collection : Xunit.ICollectionFixture<{className}>
    {{
    }}

    [XunitCollection(""{className}"")]
    public abstract class {className}Assertions<TCaseArrangementFixture> : Jds.TestingUtils.Xunit2.Extras.BaseCaseAssertions<TCaseArrangementFixture>
      where TCaseArrangementFixture : {className}Fixture
    {{
        protected {className}Assertions(TCaseArrangementFixture fixture, XunitTestOutputHelper testOutputHelper)
          : base(fixture, testOutputHelper)
        {{
        }}
    }}

    /// <summary>An <c>abstract</c> base test case arrangement fixture which has access to the shared <see cref=""{className}"" /> context object.</summary>
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
    ///     When xUnit constructs this class it will invokes those methods once for this fixture.
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
    public abstract class {className}Fixture : JdsCaseArrangementFixture
    {{
        /// <summary>Gets the shared <see cref=""{className}"" /> object.</summary>
        /// <remarks>This context is expected to contain shared, <c>readonly</c> configuration and dependencies for this fixture.</remarks>
        public {className} Context {{ get; init; }}

        protected {className}Fixture({className} context)
        {{
            Context = context;
        }}

        /// <inheritdoc />
        Task JdsCasePhases.ActAsync()
        {{
            return this.ActAsync();
        }}

        /// <inheritdoc />
        Task JdsCasePhases.ArrangeAsync()
        {{
            return this.ArrangeAsync();
        }}

        /// <inheritdoc />
        Task JdsCasePhases.AcquireSanityValuesAsync()
        {{
            return this.AcquireSanityValuesAsync();
        }}

        /// <inheritdoc />
        Task JdsCasePhases.AcquireVerificationValuesAsync()
        {{
            return this.AcquireVerificationValuesAsync();
        }}

        /// <inheritdoc />
        Task JdsDestructiveCase.CleanupAsync()
        {{
            return this.CleanupAsync();
        }}

        /// <inheritdoc cref=""Jds.TestingUtils.Xunit2.Extras.ICasePhases.ArrangeAsync""/>
        protected virtual Task ArrangeAsync()
        {{
            return Task.CompletedTask;
        }}

        /// <inheritdoc cref=""Jds.TestingUtils.Xunit2.Extras.ICasePhases.AcquireSanityValuesAsync""/>
        protected virtual Task AcquireSanityValuesAsync()
        {{
            return Task.CompletedTask;
        }}

        /// <inheritdoc cref=""Jds.TestingUtils.Xunit2.Extras.ICasePhases.ActAsync""/>
        protected virtual Task ActAsync()
        {{
            return Task.CompletedTask;
        }}

        /// <inheritdoc cref=""Jds.TestingUtils.Xunit2.Extras.ICasePhases.AcquireVerificationValuesAsync""/>
        protected virtual Task AcquireVerificationValuesAsync()
        {{
            return Task.CompletedTask;
        }}

        /// <inheritdoc cref=""Jds.TestingUtils.Xunit2.Extras.IDestructiveCase.CleanupAsync""/>
        protected virtual Task CleanupAsync()
        {{
            return Task.CompletedTask;
        }}
    }}
}}");
      return builder.ToString();
    }
  }

  private class SyntaxReceiver : ISyntaxReceiver
  {
    public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
      if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax
          && classDeclarationSyntax.AttributeLists.Count > 0)
        foreach (var attributeList in classDeclarationSyntax.AttributeLists)
        foreach (var attribute in attributeList.Attributes)
          if (attribute.Name.ToString() == "SharedCaseContext")
          {
            CandidateClasses.Add(classDeclarationSyntax);
            break;
          }
    }
  }
}
