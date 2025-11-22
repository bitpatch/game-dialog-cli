namespace DialogLang.Tests;

public class BlankLineTests
{
    [Fact]
    public void ScriptEndingWithBlankLine()
    {
        // Arrange
        var script = "<< \"Hello!\"\n"; // Script with trailing newline

        // Act & Assert
        var output = Utils.Execute(script);
        Assert.Single(output);
        Assert.Equal("Hello!", output[0]);
    }

    [Fact]
    public void ScriptEndingWithMultipleBlankLines()
    {
        // Arrange
        var script = "<< \"Hello!\"\n\n\n"; // Script with multiple trailing newlines

        // Act & Assert
        var output = Utils.Execute(script);
        Assert.Single(output);
        Assert.Equal("Hello!", output[0]);
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

        // Act & Assert
        var output = Utils.Execute(script);
        Assert.Single(output);
        Assert.Equal("Hello!", output[0]);
    }

    [Fact]
    public void BlankLinesBetweenStatements()
    {
        // Arrange
        var script = "<< \"First\"\n\n\n<< \"Second\"\n";

        // Act & Assert
        var output = Utils.Execute(script);
        Assert.Equal(2, output.Count);
        Assert.Equal("First", output[0]);
        Assert.Equal("Second", output[1]);
    }
}