namespace Lang.Tests;

public class MultistringTests
{
    [Theory]
    [InlineData("<< \"\"\"\"Hello\"\"\"\"", "Hello")]
    [InlineData("<< \"\"\"Hello, World!\"\"\"", "Hello, World!")]
    [InlineData("<< \"\"\"{42}\"\"\"", "42")]
    [InlineData("<< \"\"\"{10 + 5}\"\"\"", "15")]
    [InlineData("<< \"\"\"Result: {100}\"\"\"", "Result: 100")]
    [InlineData("<< \"\"\"a={1}, b={2}\"\"\"", "a=1, b=2")]
    [InlineData("<< \"\"\"{\"nested\"}\"\"\"", "nested")]
    [InlineData("<< \"\"\"{5 > 3}\"\"\"", "true")]
    [InlineData("<< \"\"\"Check: {5 > 3}\"\"\"", "Check: true")]
    [InlineData("<< \"\"\"Check:\n  {5 > 3}\"\"\"", "Check:\ntrue")]
    [InlineData("<< \"\"\"Check:\n  {5 > 3}\n   hello\"\"\"", "Check:\ntrue\n hello")]
    public void SimpleStrings(string source, string expected)
    {
        // Act
        var result = Utils.Execute(source);

        // Assert
        result.AssertEqual(expected);
    }

    [Fact]
    public void WithVariable()
    {
        // Arrange
        var source = """"""
            name = "World"
            << """"{name} is """grateful."""
               """"
            """""";

        // Act
        var result = Utils.Execute(source);

        // Assert
        result.AssertEqual("World is \"\"\"grateful.\"\"\"\n");
    }

    [Fact]
    public void VariableInText()
    {
        // Arrange
        var source = """"
            name = "World"
            << """Hello,
               {name}
               !"""
            """";

        // Act
        var result = Utils.Execute(source);

        // Assert
        result.AssertEqual("Hello,\nWorld\n!");
    }

    [Fact]
    public void WithArithmeticExpression()
    {
        // Arrange
        var source = """"
            x = 10
            y = 5
            << """Sum:
               {x + y}"""
            """";

        // Act
        var result = Utils.Execute(source);

        // Assert
        result.AssertEqual("Sum:\n15");
    }

    [Fact]
    public void InIfBlock()
    {
        // Arrange
        var source = """"
            show = true
            if show
                << """Shown!
                      Shown!
                      Shown!"""
            """";

        // Act
        var result = Utils.Execute(source);

        // Assert
        result.AssertEqual("Shown!\nShown!\nShown!");
    }
}
