using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class InputLexerTests
{
    [Fact]
    public void Input()
    {
        // Arrange
        var source = ">> a";

        var expected = new[]
        {
            TokenType.Input, TokenType.Identifier, TokenType.Newline,
            TokenType.EndOfSource
        };

        // Act
        var result = source.Tokenize();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void InputWithMultipleIdentifiers()
    {
        // Arrange
        var source = """
        >> name
        >> age
        """;

        var expected = new[]
        {
            TokenType.Input, TokenType.Identifier, TokenType.Newline,
            TokenType.Input, TokenType.Identifier, TokenType.Newline,
            TokenType.EndOfSource
        };

        // Act
        var result = source.Tokenize();

        // Assert
        Assert.Equal(expected, result);
    }
}

