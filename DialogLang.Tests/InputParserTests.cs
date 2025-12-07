using BitPatch.DialogLang.Ast;

namespace DialogLang.Tests;

public class InputParserTests
{
    [Fact]
    public void Input()
    {
        // Arrange
        var source = ">> name";

        var expected = new[]
        {
            typeof(Input)
        };

        // Act
        var result = source.Parse().AddTypesTo([]);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void InputWithMultipleStatements()
    {
        // Arrange
        var source = """
        >> name
        >> age
        << name
        """;

        var expected = new[]
        {
            typeof(Input),  // >> name
            typeof(Input),  // >> age
            typeof(Output)  // << name
        };

        // Act
        var result = source.Parse().AddTypesTo([]);

        // Assert
        Assert.Equal(expected, result);
    }
}
