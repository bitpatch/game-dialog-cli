using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class ArithmeticTests
{
    [Theory]
    [InlineData("<< 0 + 0", 0)]
    [InlineData("<< 5 + 3", 8)]
    [InlineData("<< 5 - 3", 2)]
    public void IntegerArithmetic(string script, int expected)
    {
        // Act
        var results = Utils.Execute(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Theory]
    [InlineData("<< 0 + 0.001", 0.001f)]
    [InlineData("<< 0 - 0.001", -0.001f)]
    [InlineData("<< 2 + 2.0", 4.0f)]
    [InlineData("<< 3.5 + 1.5", 5.0f)]
    [InlineData("<< 10.5 - 2.5", 8.0f)]
    public void FloatArithmetic(string script, float expected)
    {
        // Act
        var results = Utils.Execute(script);

        // Assert
        Assert.Single(results);
        Assert.Equal(expected, (float)results[0], precision: 5);
    }

    [Theory]
    [InlineData("<< (5 + 3) - 2", 6)]
    [InlineData("<< 5 + (3 - 2)", 6)]
    [InlineData("<< (10 - 5) + (8 - 3)", 10)]
    [InlineData("<< 100 - (50 + 25)", 25)]
    public void ParenthesesPrecedence(string script, int expected)
    {
        // Act
        var results = Utils.Execute(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }
}
