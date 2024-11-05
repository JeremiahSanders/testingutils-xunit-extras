using FluentAssertions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Jds.TestingUtils.Xunit2.Extras.Tests.Unit;

public class SharedCaseContextGeneratorTests
{
  public class CreateCollectionSourceTests
  {
    private const string expectedSource = """
                                          namespace MyOrganization.MyProject.TestNamespace
                                          {
                                              [Xunit.CollectionDefinition("TestClass")]
                                              public class TestClassCollection : Xunit.ICollectionFixture<TestClass>
                                              {
                                              }
                                          }
                                          """;

    [Fact]
    public void Should_Return_Correct_Source_With_Block_Scoped_Namespace()
    {
      // Arrange
      var source = @"
namespace MyOrganization.MyProject.TestNamespace
{
    [SharedCaseContext]
    public class TestClass
    {
    }
}";
      var syntaxTree = CSharpSyntaxTree.ParseText(source);
      var classDeclaration =
        (ClassDeclarationSyntax)syntaxTree.GetRoot().DescendantNodes().First(node => node is ClassDeclarationSyntax);

      // Act
      var (hintName, sourceCode) = SharedCaseContextGenerator.CreateCollectionSource(classDeclaration);


      // Assert
      hintName.Should().Be("TestClassCollection");
      sourceCode.Trim().Should().Be(expectedSource.Trim());
    }

    [Fact]
    public void Should_Return_Correct_Source_With_File_Scoped_Namespace()
    {
      // Arrange
      var source = @"
namespace MyOrganization.MyProject.TestNamespace;

[SharedCaseContext]
public class TestClass
{
}";
      var syntaxTree = CSharpSyntaxTree.ParseText(source);
      var classDeclaration =
        (ClassDeclarationSyntax)syntaxTree.GetRoot().DescendantNodes().First(node => node is ClassDeclarationSyntax);

      // Act
      var (hintName, sourceCode) = SharedCaseContextGenerator.CreateCollectionSource(classDeclaration);

      // Assert
      hintName.Should().Be("TestClassCollection");
      sourceCode.Trim().Should().Be(expectedSource.Trim());
    }
  }

  public class CreateAssertionsSourceTests
  {
    private const string expectedSource = @"
namespace MyOrganization.MyProject.TestNamespace
{
    [Xunit.Collection(""TestClass"")]
    public abstract class TestClassAssertions<TCaseArrangementFixture> : Jds.TestingUtils.Xunit2.Extras.BaseCaseAssertions<TCaseArrangementFixture>
      where TCaseArrangementFixture : TestClassFixture
    {
        protected TestClassAssertions(TCaseArrangementFixture fixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
          : base(fixture, testOutputHelper)
        {
        }
    }
}";

    [Fact]
    public void Should_Return_Correct_Source_With_Block_Scoped_Namespace()
    {
      // Arrange
      var source = @"
namespace MyOrganization.MyProject.TestNamespace
{
    [SharedCaseContext]
    public class TestClass
    {
    }
}";
      var syntaxTree = CSharpSyntaxTree.ParseText(source);
      var classDeclaration =
        (ClassDeclarationSyntax)syntaxTree.GetRoot().DescendantNodes().First(node => node is ClassDeclarationSyntax);

      // Act
      var (hintName, sourceCode) = SharedCaseContextGenerator.CreateAssertionsSource(classDeclaration);

      // Assert
      hintName.Should().Be("TestClassAssertions");
      sourceCode.Trim().Should().Be(expectedSource.Trim());
    }

    [Fact]
    public void Should_Return_Correct_Source_With_File_Scoped_Namespace()
    {
      // Arrange
      var source = @"
namespace MyOrganization.MyProject.TestNamespace;

[SharedCaseContext]
public class TestClass
{
}";
      var syntaxTree = CSharpSyntaxTree.ParseText(source);
      var classDeclaration =
        (ClassDeclarationSyntax)syntaxTree.GetRoot().DescendantNodes().First(node => node is ClassDeclarationSyntax);

      // Act
      var (hintName, sourceCode) = SharedCaseContextGenerator.CreateAssertionsSource(classDeclaration);


      // Assert
      hintName.Should().Be("TestClassAssertions");
      sourceCode.Trim().Should().Be(expectedSource.Trim());
    }
  }

  public class CreateFixtureSourceTests
  {
    private const string expectedSource = @"
using JdsCasePhases = Jds.TestingUtils.Xunit2.Extras.ICasePhases;
using JdsDestructiveCase = Jds.TestingUtils.Xunit2.Extras.IDestructiveCase;
using JdsCaseArrangementFixture = Jds.TestingUtils.Xunit2.Extras.ICaseArrangementFixture;

namespace MyOrganization.MyProject.TestNamespace
{
    /// <summary>An <c>abstract</c> base test case arrangement fixture which has access to the shared <see cref=""TestClass"" /> context object.</summary>
    /// <remarks>
    ///   <para>How to use:</para>
    ///   <para>
    ///     Override the <see cref=""ArrangeAsync"" />, <see cref=""AcquireSanityValuesAsync"" />,
    ///     <see cref=""ActAsync"" />, <see cref=""AcquireVerificationValuesAsync"" />,
    ///     and <see cref=""CleanupAsync"" /> methods as needed.
    ///     Then, create an &quot;assertion/test&quot; class, based on <see cref=""TestClassAssertions{TCaseArrangementFixture}"" />,
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
    ///     require no constructor parameters other than <see cref=""TestClass"" />.
    ///   </para>
    /// </remarks>
    public abstract class TestClassFixture : JdsCaseArrangementFixture
    {
        /// <summary>Gets the shared <see cref=""TestClass"" /> object.</summary>
        /// <remarks>This context is expected to contain shared, <c>readonly</c> configuration and dependencies for this fixture.</remarks>
        public TestClass Context { get; init; }

        protected TestClassFixture(TestClass context)
        {
            Context = context;
        }

        /// <inheritdoc />
        Task JdsCasePhases.ActAsync()
        {
            return this.ActAsync();
        }

        /// <inheritdoc />
        Task JdsCasePhases.ArrangeAsync()
        {
            return this.ArrangeAsync();
        }

        /// <inheritdoc />
        Task JdsCasePhases.AcquireSanityValuesAsync()
        {
            return this.AcquireSanityValuesAsync();
        }

        /// <inheritdoc />
        Task JdsCasePhases.AcquireVerificationValuesAsync()
        {
            return this.AcquireVerificationValuesAsync();
        }

        /// <inheritdoc />
        Task JdsDestructiveCase.CleanupAsync()
        {
            return this.CleanupAsync();
        }

        /// <inheritdoc cref=""Jds.TestingUtils.Xunit2.Extras.ICasePhases.ArrangeAsync""/>
        protected virtual Task ArrangeAsync()
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc cref=""Jds.TestingUtils.Xunit2.Extras.ICasePhases.AcquireSanityValuesAsync""/>
        protected virtual Task AcquireSanityValuesAsync()
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc cref=""Jds.TestingUtils.Xunit2.Extras.ICasePhases.ActAsync""/>
        protected virtual Task ActAsync()
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc cref=""Jds.TestingUtils.Xunit2.Extras.ICasePhases.AcquireVerificationValuesAsync""/>
        protected virtual Task AcquireVerificationValuesAsync()
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc cref=""Jds.TestingUtils.Xunit2.Extras.IDestructiveCase.CleanupAsync""/>
        protected virtual Task CleanupAsync()
        {
            return Task.CompletedTask;
        }
    }
}";

    [Fact]
    public void Should_Return_Correct_Source_With_Block_Scoped_Namespace()
    {
      // Arrange
      var source = @"
namespace MyOrganization.MyProject.TestNamespace
{
    [SharedCaseContext]
    public class TestClass
    {
    }
}";
      var syntaxTree = CSharpSyntaxTree.ParseText(source);
      var classDeclaration =
        (ClassDeclarationSyntax)syntaxTree.GetRoot().DescendantNodes().First(node => node is ClassDeclarationSyntax);

      // Act
      var (hintName, sourceCode) = SharedCaseContextGenerator.CreateFixtureSource(classDeclaration);


      // Assert
      hintName.Should().Be("TestClassFixture");
      sourceCode.Trim().Should().Be(expectedSource.Trim());
    }

    [Fact]
    public void Should_Return_Correct_Source_With_File_Scoped_Namespace()
    {
      // Arrange
      var source = @"
namespace MyOrganization.MyProject.TestNamespace;

[SharedCaseContext]
public class TestClass
{
}";
      var syntaxTree = CSharpSyntaxTree.ParseText(source);
      var classDeclaration =
        (ClassDeclarationSyntax)syntaxTree.GetRoot().DescendantNodes().First(node => node is ClassDeclarationSyntax);

      // Act
      var (hintName, sourceCode) = SharedCaseContextGenerator.CreateFixtureSource(classDeclaration);

      // Assert
      hintName.Should().Be("TestClassFixture");
      sourceCode.Trim().Should().Be(expectedSource.Trim());
    }
  }
}
