using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class MultistringTokenTests
{
    [Fact]
    public void SimpleString()
    {
        // Arrange
        var source = "<< \"\"\"Hello, World!\"\"\"\t\t";

        var expected = new TokenSequence(
        [
            TokenType.Output, TokenType.StringStart, TokenType.InlineString, TokenType.StringEnd, TokenType.Newline,
            TokenType.EndOfSource
        ]);

        // Act
        var result = source.Tokenize();

        // Assert
        Assert.Equal(expected.Sequence, result);
    }

    [Fact]
    public void TwoStrings()
    {
        // Arrange
        var source = """"
            << """
                Hello,
                World!
                """
            """";

        var expected = new TokenSequence(
        [
            TokenType.Output, TokenType.StringStart,
            TokenType.InlineString,
            TokenType.InlineString, TokenType.StringEnd, TokenType.Newline,
            TokenType.EndOfSource
        ]);

        // Act
        var result = source.Tokenize();

        // Assert
        Assert.Equal(expected.Sequence, result);

    }

    [Fact]
    public void TwoStringsAndExpression()
    {
        // Arrange
        var source = """"
            << """
                Hello {"World!"}
                Hello!
                """
            """";

        var expected = new TokenSequence(
        [
            TokenType.Output, TokenType.StringStart,
            TokenType.InlineString, TokenType.InlineExpressionStart, TokenType.InlineString, TokenType.InlineExpressionEnd, TokenType.InlineString,
            TokenType.InlineString, TokenType.StringEnd, TokenType.Newline,
            TokenType.EndOfSource
        ]);

        // Act
        var result = source.Tokenize();

        // Assert
        Assert.Equal(expected.Sequence, result);

    }

    [Fact]
    public void InIfBlock()
    {
        // Arrange
        var source = """"
            if true
                << """Hello,
                   World!
                   """
            """";

        var expected = new TokenSequence(
        [
            TokenType.If, TokenType.True, TokenType.Newline,
            TokenType.Indent,
                TokenType.Output, TokenType.StringStart,
                    TokenType.InlineString,
                    TokenType.InlineString,
                TokenType.StringEnd, TokenType.Newline,
            TokenType.Dedent,
            TokenType.EndOfSource
        ]);

        // Act
        var result = source.Tokenize();

        // Assert
        Assert.Equal(expected.Sequence, result);
    }
}