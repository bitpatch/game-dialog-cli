using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class ArithmeticTests
{
    private static List<object> ExecuteScript(string script)
    {
        var dialog = new Dialog();
        return dialog.Execute(script).ToList();
    }

    [Theory]
    [InlineData("<< 5 + 3", 8)]
    [InlineData("<< 10 + 20", 30)]
    [InlineData("<< 0 + 0", 0)]
    [InlineData("<< 100 + 1", 101)]
    public void IntegerAddition(string script, int expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Theory]
    [InlineData("<< 10 - 3", 7)]
    [InlineData("<< 20 - 5", 15)]
    [InlineData("<< 5 - 5", 0)]
    [InlineData("<< 0 - 10", -10)]
    [InlineData("<< 100 - 99", 1)]
    public void IntegerSubtraction(string script, int expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Theory]
    [InlineData("<< 3.5 + 2.5", 6.0f)]
    [InlineData("<< 10.1 + 5.2", 15.3f)]
    [InlineData("<< 0.0 + 0.0", 0.0f)]
    [InlineData("<< 1.5 + 1.5", 3.0f)]
    public void FloatAddition(string script, float expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Single(results);
        Assert.Equal(expected, (float)results[0], precision: 5);
    }

    [Theory]
    [InlineData("<< 10.5 - 3.2", 7.3f)]
    [InlineData("<< 20.0 - 5.5", 14.5f)]
    [InlineData("<< 5.5 - 5.5", 0.0f)]
    [InlineData("<< 0.0 - 10.5", -10.5f)]
    public void FloatSubtraction(string script, float expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Single(results);
        Assert.Equal(expected, (float)results[0], precision: 5);
    }

    [Theory]
    [InlineData("<< 5 + 3.5", 8.5f)]
    [InlineData("<< 10.5 + 20", 30.5f)]
    [InlineData("<< 100 + 0.5", 100.5f)]
    [InlineData("<< 3.2 + 7", 10.2f)]
    public void MixedAddition(string script, float expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Single(results);
        Assert.Equal(expected, (float)results[0], precision: 5);
    }

    [Theory]
    [InlineData("<< 10 - 3.5", 6.5f)]
    [InlineData("<< 20.5 - 5", 15.5f)]
    [InlineData("<< 100.0 - 50", 50.0f)]
    [InlineData("<< 7 - 3.2", 3.8f)]
    public void MixedSubtraction(string script, float expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Single(results);
        Assert.Equal(expected, (float)results[0], precision: 5);
    }

    [Theory]
    [InlineData("<< 1 + 2 + 3", 6)]
    [InlineData("<< 10 - 5 - 2", 3)]
    [InlineData("<< 10 + 5 - 3", 12)]
    [InlineData("<< 20 - 10 + 5", 15)]
    [InlineData("<< 1 + 2 + 3 + 4", 10)]
    public void ChainedOperations(string script, int expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Theory]
    [InlineData("<< (5 + 3) - 2", 6)]
    [InlineData("<< 5 + (3 - 2)", 6)]
    [InlineData("<< (10 - 5) + (8 - 3)", 10)]
    [InlineData("<< 100 - (50 + 25)", 25)]
    public void ParenthesesPrecedence(string script, int expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Fact]
    public void VariablesWithArithmetic()
    {
        var results = ExecuteScript("""
            a = 10
            b = 20
            c = a + b
            << c
            d = c - 5
            << d
            """);

        Assert.Equal(new object[] { 30, 25 }, results);
    }

    [Fact]
    public void ArithmeticWithComparison()
    {
        var results = ExecuteScript("""
            << 5 + 3 > 7
            << 10 - 5 < 3
            << 2 + 2 == 4
            << 10 - 1 != 8
            """);

        Assert.Equal(new object[] { true, false, true, true }, results);
    }

    [Fact]
    public void ArithmeticWithLogicalOperators()
    {
        var results = ExecuteScript("""
            << (5 + 5 == 10) and (3 - 1 == 2)
            << (10 - 5 > 10) or (2 + 2 == 4)
            << not (5 + 5 == 11)
            """);

        Assert.Equal(new object[] { true, true, true }, results);
    }
}
