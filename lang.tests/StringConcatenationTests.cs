using BitPatch.DialogLang;

namespace Lang.Tests;

public class StringConcatenationTests
{
    [Theory]
    [InlineData("<< \"Hello\" + \"World\"", "HelloWorld")]
    [InlineData("<< \"Hello \" + \"World\"", "Hello World")]
    [InlineData("<< \"\" + \"test\"", "test")]
    [InlineData("<< \"test\" + \"\"", "test")]
    [InlineData("<< \"\" + \"\"", "")]
    public void StringToString(string script, string expected)
    {
        // Act
        var results = Utils.Execute(script);

        // Assert
        results.AssertEqual(expected);
    }

    [Theory]
    [InlineData("<< \"Count: \" + 42", "Count: 42")]
    [InlineData("<< \"Value: \" + 0", "Value: 0")]
    [InlineData("<< 100 + \" items\"", "100 items")]
    public void StringWithInteger(string script, string expected)
    {
        // Act
        var results = Utils.Execute(script);

        // Assert
        results.AssertEqual(expected);
    }

    [Theory]
    [InlineData("<< \"Pi: \" + 3.14", "Pi: 3.14")]
    [InlineData("<< \"Value: \" + 0.0", "Value: 0")]
    [InlineData("<< \"Result: \" + 2.5", "Result: 2.5")]
    [InlineData("<< 99.9 + \" percent\"", "99.9 percent")]
    public void StringWithFloat(string script, string expected)
    {
        // Act
        var results = Utils.Execute(script);

        // Assert
        results.AssertEqual(expected);
    }

    [Theory]
    [InlineData("<< \"Active: \" + true", "Active: true")]
    [InlineData("<< \"Enabled: \" + false", "Enabled: false")]
    [InlineData("<< true + \" is truth\"", "true is truth")]
    [InlineData("<< false + \" is not true\"", "false is not true")]
    public void StringWithBoolean(string script, string expected)
    {
        // Act
        var results = Utils.Execute(script);

        // Assert
        results.AssertEqual(expected);
    }

    [Theory]
    [InlineData("<< \"Result: \" + 10 + 20", "Result: 1020")]
    [InlineData("<< 10 + 20 + \" items\"", "30 items")]
    [InlineData("<< \"Count: \" + (10 + 20)", "Count: 30")]
    [InlineData("<< (10 + 20) + \" total\"", "30 total")]
    public void MixedWithArithmetic(string script, string expected)
    {
        // Act
        var results = Utils.Execute(script);

        // Assert
        results.AssertEqual(expected);
    }

    [Theory]
    [InlineData("<< \"Hello \" + \"World\" + \"!\"", "Hello World!")]
    [InlineData("<< \"A\" + \"B\" + \"C\" + \"D\"", "ABCD")]
    [InlineData("<< \"Value: \" + 10 + \", Status: \" + true", "Value: 10, Status: true")]
    public void ChainedConcatenation(string script, string expected)
    {
        // Act
        var results = Utils.Execute(script);

        // Assert
        results.AssertEqual(expected);
    }

    [Fact]
    public void SubtractionDoesNotWorkWithStrings()
    {
        // Subtraction is only for numbers, not strings
        Assert.Throws<RuntimeError>(() => Utils.Execute("<< \"test\" - 5"));
    }
}
