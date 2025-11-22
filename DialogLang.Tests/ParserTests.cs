using BitPatch.DialogLang.Ast;

namespace DialogLang.Tests;

public class ParserTests
{
    [Fact]
    public void Block()
    {
        // Arrange
        var source = """
        x = 1
            y = 2
            << y
                << y + 1
        << x
        """;

        var expected = new[]
        {
            typeof(Assign), // x = 1
            typeof(Block),  // block:
            typeof(Assign), //   y = 2
            typeof(Output), //   << y
            typeof(Block),  //     block:
            typeof(Output), //     << y + 1
            typeof(Output)  // << x
        };

        // Act
        var result = source.Parse().AddTypesTo([]);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void WhileLoop()
    {
        // Arrange
        var source = """
        x = 3
        while x > 0
            << x
            x = x - 1
        """;

        var expected = new[]
        {
            typeof(Assign), // x = 3
            typeof(While),  // while x > 0
            typeof(Block),  // block:
            typeof(Output), //   << x
            typeof(Assign)  //   x = x - 1
        };

        // Act
        var result = source.Parse().AddTypesTo([]);

        // Assert
        Assert.Equal(expected, result);
    }
}
