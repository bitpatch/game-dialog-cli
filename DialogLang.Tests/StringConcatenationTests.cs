using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class StringConcatenationTests
{
    private static List<object> ExecuteScript(string script)
    {
        var dialog = new Dialog();
        return dialog.Execute(script).ToList();
    }

    [Theory]
    [InlineData("<< \"Hello\" + \"World\"", "HelloWorld")]
    [InlineData("<< \"Hello \" + \"World\"", "Hello World")]
    [InlineData("<< \"\" + \"test\"", "test")]
    [InlineData("<< \"test\" + \"\"", "test")]
    [InlineData("<< \"\" + \"\"", "")]
    public void StringToString(string script, string expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Theory]
    [InlineData("<< \"Count: \" + 42", "Count: 42")]
    [InlineData("<< \"Value: \" + 0", "Value: 0")]
    [InlineData("<< 100 + \" items\"", "100 items")]
    public void StringWithInteger(string script, string expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Theory]
    [InlineData("<< \"Pi: \" + 3.14", "Pi: 3.14")]
    [InlineData("<< \"Value: \" + 0.0", "Value: 0")]
    [InlineData("<< \"Result: \" + 2.5", "Result: 2.5")]
    [InlineData("<< 99.9 + \" percent\"", "99.9 percent")]
    public void StringWithFloat(string script, string expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Theory]
    [InlineData("<< \"Active: \" + true", "Active: true")]
    [InlineData("<< \"Enabled: \" + false", "Enabled: false")]
    [InlineData("<< true + \" is truth\"", "true is truth")]
    [InlineData("<< false + \" is not true\"", "false is not true")]
    public void StringWithBoolean(string script, string expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Theory]
    [InlineData("<< \"Result: \" + 10 + 20", "Result: 1020")]
    [InlineData("<< 10 + 20 + \" items\"", "30 items")]
    [InlineData("<< \"Count: \" + (10 + 20)", "Count: 30")]
    [InlineData("<< (10 + 20) + \" total\"", "30 total")]
    public void MixedWithArithmetic(string script, string expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Theory]
    [InlineData("<< \"Hello \" + \"World\" + \"!\"", "Hello World!")]
    [InlineData("<< \"A\" + \"B\" + \"C\" + \"D\"", "ABCD")]
    [InlineData("<< \"Value: \" + 10 + \", Status: \" + true", "Value: 10, Status: true")]
    public void ChainedConcatenation(string script, string expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Fact]
    public void VariablesWithConcatenation()
    {
        var results = ExecuteScript("""
            name = "Alice"
            age = 25
            active = true
            << "Name: " + name
            << "Age: " + age
            << "Active: " + active
            message = "Hello, " + name + "!"
            << message
            """);

        Assert.Equal(new object[] { "Name: Alice", "Age: 25", "Active: true", "Hello, Alice!" }, results);
    }

    [Fact]
    public void ConcatenationWithComparisonInParentheses()
    {
        var results = ExecuteScript("""
            << "Result: " + (5 > 3)
            << "Equal: " + (10 == 10)
            << "Check: " + (true and false)
            """);

        Assert.Equal(new object[] { "Result: true", "Equal: true", "Check: false" }, results);
    }

    [Fact]
    public void PrecedenceWithLogicalOperators()
    {
        // String concatenation has higher precedence than logical operators
        // "Result: " + (y and false) should work
        // "Result: " + y and false should fail (string in logical operation)
        
        var results = ExecuteScript("""
            y = true
            << "Result: " + (y and false)
            << "Value: " + (true or false)
            """);

        Assert.Equal(new object[] { "Result: false", "Value: true" }, results);
    }

    [Fact]
    public void SubtractionDoesNotWorkWithStrings()
    {
        // Subtraction is only for numbers, not strings
        Assert.Throws<TypeMismatchException>(() => ExecuteScript("<< \"test\" - 5"));
    }
}
