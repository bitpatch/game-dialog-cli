using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class LexerTests
{
    private static List<Token> Tokenize(string source)
    {
        using var reader = new StringReader(source);
        var lexer = new BitPatch.DialogLang.Lexer(reader);
        return [.. lexer.Tokenize()];
    }

    [Fact]
    public void Indentation()
    {
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
        var result = Tokenize(source);

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
            TokenType.EndOfFile
        };

        Assert.Equal(expected, result.Select(t => t.Type));
    }
}

