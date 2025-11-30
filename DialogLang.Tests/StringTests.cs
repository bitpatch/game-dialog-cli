using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class StringTests
{
    [Fact]
    public void TokenizeSimpleString()
    {
        // Arrange
        var source = "<< \"Hello\"";

        var expected = new[]
        {
            TokenType.Output, TokenType.StringStart,
                TokenType.InlineString,
            TokenType.StringEnd, TokenType.Newline,
            TokenType.EndOfSource
        };

        // Act
        var result = source.Tokenize();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TokenizeExpression()
    {
        // Arrange
        var source = "<< \"{42}\"";

        var expected = new[]
        {
            TokenType.Output, TokenType.StringStart,
                TokenType.InlineExpressionStart,
                    TokenType.Integer,
                TokenType.InlineExpressionEnd,
            TokenType.StringEnd, TokenType.Newline,
            TokenType.EndOfSource
        };

        // Act
        var result = source.Tokenize();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TokenizeStringAndExpression()
    {
        // Arrange
        var source = "<< \"a = {42}\"";

        var expected = new[]
        {
            TokenType.Output, TokenType.StringStart,
                TokenType.InlineString,
                TokenType.InlineExpressionStart,
                    TokenType.Integer,
                TokenType.InlineExpressionEnd,
            TokenType.StringEnd, TokenType.Newline,
            TokenType.EndOfSource
        };

        // Act
        var result = source.Tokenize();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void TokenizeInlineString()
    {
        // Arrange
        var source = "<< \"{\"Hello\"}\"";

        var expected = new[]
        {
            TokenType.Output, TokenType.StringStart,
                TokenType.InlineExpressionStart,
                    TokenType.InlineString,
                TokenType.InlineExpressionEnd,
            TokenType.StringEnd, TokenType.Newline,
            TokenType.EndOfSource
        };

        // Act
        var result = source.Tokenize();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("<< \"Hello\"", "Hello")]
    [InlineData("<< \"{42}\"", "42")]
    [InlineData("<< \"{10 + 5}\"", "15")]
    [InlineData("<< \"{5}{5}\"", "55")]
    [InlineData("<< \"{5}{5 > 2}\"", "5true")]
    [InlineData("<< \"pi = {3.14}\"", "pi = 3.14")]
    [InlineData("<< \"a = {10}, b = {20}\"", "a = 10, b = 20")]
    [InlineData("<< \"{\"Hello\"}\"", "Hello")]
    [InlineData("<< \"{\"Hello\"} {10 + 1.12}\"", "Hello 11.12")]
    [InlineData("<< \"{5 > 3}\"", "true")]
    [InlineData("<< \"5 > 3: {5 > 3}\"", "5 > 3: true")]
    [InlineData("<< \"Hi {5 + 3}, {5 > 3}\"", "Hi 8, true")]
    public void Strings(string source, string expected)
    {
        // Act
        var result = Utils.Execute(source);

        // Assert
        result.AssertEqual(expected);
    }

    [Fact]
    public void SimpleVariableInterpolation()
    {
        // Arrange
        var source = """
        name = "World"
        << "Hello, {name}!"
        """;

        // Act
        var results = Utils.Execute(source);

        // Assert
        results.AssertEqual("Hello, World!");
    }

    [Fact]
    public void MultipleVariables()
    {
        // Arrange
        var source = """
        a = 10
        b = 20
        << "a = {a}, b = {b}"
        """;

        // Act
        var results = Utils.Execute(source);

        // Assert
        results.AssertEqual("a = 10, b = 20");
    }

    [Fact]
    public void ComplexExpression()
    {
        // Arrange
        var source = """
        x = 5
        y = 3
        << "Result: {x * y + 10}, {x > y}"
        """;

        // Act
        var results = Utils.Execute(source);

        // Assert
        results.AssertEqual("Result: 25, true");
    }
}
