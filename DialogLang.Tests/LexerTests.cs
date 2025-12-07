using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class LexerTests
{
    [Fact]
    public void Indentation()
    {
        // Arrange
        var source = """
        x = 1
        # Some text
            y = 2 #Comments
        << x
           # Comment
           << y
        # Comment
          # Comment

            z = 3
        """;

        var expected = new[]
{
            TokenType.Identifier, TokenType.Assign, TokenType.Integer, TokenType.Newline,
            TokenType.Indent,
            TokenType.Identifier, TokenType.Assign, TokenType.Integer, TokenType.Newline,
            TokenType.Dedent,
            TokenType.Output, TokenType.Identifier, TokenType.Newline,
            TokenType.Indent,
            TokenType.Output, TokenType.Identifier, TokenType.Newline,
            TokenType.Indent,
            TokenType.Identifier, TokenType.Assign, TokenType.Integer, TokenType.Newline,
            TokenType.Dedent,
            TokenType.Dedent,
            TokenType.EndOfSource
        };

        // Act
        var result = source.Tokenize();

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
            TokenType.Identifier, TokenType.Assign, TokenType.Integer, TokenType.Newline,
            TokenType.While, TokenType.Identifier, TokenType.GreaterThan, TokenType.Integer, TokenType.Newline,
            TokenType.Indent,
            TokenType.Output, TokenType.Identifier, TokenType.Newline,
            TokenType.Identifier, TokenType.Assign, TokenType.Identifier, TokenType.Minus, TokenType.Integer, TokenType.Newline,
            TokenType.Dedent,
            TokenType.EndOfSource
        };

        // Act
        var result = source.Tokenize();

        // Assert
        Assert.Equal(expected, result);
    }
}
