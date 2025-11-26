using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class BlankLineTests
{
    [Fact]
    public void EndsWithBlankLineParsing()
    {
        // Arrange
        var script = "<< \"Hello!\"\n"; // Script with trailing newline

        var expected = new[]
        {
            TokenType.Output, TokenType.StringStart, TokenType.InlineString, TokenType.StringEnd,
            TokenType.Newline, TokenType.EndOfSource
        };

        // Act
        var output = script.Tokenize();

        // Assert
        Assert.Equal(expected, output);
    }

    [Fact]
    public void EndsWithBlankLine()
    {
        // Arrange
        var script = "<< \"Hello!\"\n"; // Script with trailing newline

        // Act
        var output = Utils.Execute(script);

        // Assert
        output.AssertEqual("Hello!");
    }

    [Fact]
    public void EndsWithMultiBlankLines()
    {
        // Arrange
        var script = "<< \"Hello!\"\n\n\n"; // Script with multiple trailing newlines

        // Act
        var output = Utils.Execute(script);

        // Assert
        output.AssertEqual("Hello!");
    }

    [Fact]
    public void EmptyScript()
    {
        // Arrange
        var script = "";

        // Act & Assert
        var output = Utils.Execute(script);
        Assert.Empty(output);
    }

    [Fact]
    public void OnlyNewlines()
    {
        // Arrange
        var script = "\n\n\n";

        // Act & Assert
        var output = Utils.Execute(script);
        Assert.Empty(output);
    }

    [Fact]
    public void BlankLinesBeforeContent()
    {
        // Arrange
        var script = "\n\n<< \"Hello!\"";

        // Act
        var output = Utils.Execute(script);

        // Assert
        output.AssertEqual("Hello!");
    }

    [Fact]
    public void BlankLinesBetweenStatements()
    {
        // Arrange
        var script = "<< \"First\"\n\n\n<< \"Second\"\n";

        // Act
        var output = Utils.Execute(script);

        // Assert
        output.AssertEqual(["First", "Second"]);
    }

    [Fact]
    public void EndsWithComment()
    {
        // Arrange
        var script = "<< \"First\"\n# Comment";

        // Act
        var output = Utils.Execute(script);

        // Assert
        output.AssertEqual(["First"]);
    }
}