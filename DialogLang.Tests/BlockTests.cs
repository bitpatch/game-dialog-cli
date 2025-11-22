using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class BlockTests
{
    [Fact]
    public void SingleStatement()
    {
        // Arrange
        var script = """
            x = 5
                << x
            """;

        // Act
        var results = Utils.Execute(script);

        // Assert
        results.AssertEqual(5);
    }

    [Fact]
    public void SequentialBlocks()
    {
        // Arrange
        var script = """
            a = 1
                << a
            b = 2
                << b
            c = 3
                << c
            """;

        // Act
        var results = Utils.Execute(script);

        // Assert
        results.AssertEqual(1, 2, 3);
    }

    [Fact]
    public void NestedBlocks()
    {
        // Arrange
        var script = """
            x = 1
                y = 2
                    z = 3
                        w = x + y + z
                        << w
            """;

        // Act
        var results = Utils.Execute(script);

        // Assert
        results.AssertEqual(6);
    }

    [Fact]
    public void MismatchedDedentLevel_ThrowsError()
    {
        // Arrange - dedenting to a level that wasn't established
        var script = """
            x = 1
                y = 2
                    z = 3
                w = 4
              v = 5
            """;

        // Act & Assert
        Assert.Throws<InvalidSyntaxException>(() => Utils.Execute(script));
    }

    [Fact]
    public void EmptyLinesInsideBlocks()
    {
        // Arrange
        var script = """
            x = 1
                y = 2

                z = x + y
                << z
            """;

        // Act
        var results = Utils.Execute(script);

        // Assert
        results.AssertEqual(3);
    }

    [Fact]
    public void SingleTab()
    {
        // Arrange
        var script = "x = 1\n\ty = 2\n\t\tz = 3\n\t\t\t<< x + y + z";

        // Act
        var results = Utils.Execute(script);

        // Assert
        results.AssertEqual(6);
    }

    [Fact]
    public void SwitchingIndentationStyle()
    {
        // Arrange - start with spaces, then switch to tabs
        var script = "x = 1\n    y = 2\n\t\tz = 3";

        // Act & Assert
        Assert.Throws<InvalidSyntaxException>(() => Utils.Execute(script));
    }
}
