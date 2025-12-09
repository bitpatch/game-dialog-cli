using BitPatch.DialogLang;

namespace Lang.Tests;

public class ArithmeticTests
{
    [Theory]
    [InlineData("<< 0 + 0", 0)]
    [InlineData("<< 5 + 3", 8)]
    [InlineData("<< 5 - 3", 2)]
    [InlineData("<< 5 * 3", 15)]
    [InlineData("<< 7 * 8", 56)]
    [InlineData("<< -5", -5)]
    [InlineData("<< -(-5)", 5)]
    [InlineData("<< 10 % 3", 1)]
    [InlineData("<< 17 % 5", 2)]
    [InlineData("<< 20 % 4", 0)]
    public void Integers(string script, int expected)
    {
        // Act
        var results = Utils.Execute(script);

        // Assert
        results.AssertEqual(expected);
    }

    [Theory]
    [InlineData("<< 0 + 0.001", 0.001f)]
    [InlineData("<< 0 - 0.001", -0.001f)]
    [InlineData("<< 2 + 2.0", 4.0f)]
    [InlineData("<< 3.5 + 1.5", 5.0f)]
    [InlineData("<< 10.5 - 2.5", 8.0f)]
    [InlineData("<< 5 / 2", 2.5f)]
    [InlineData("<< 20 / 4", 5)]
    [InlineData("<< 2.5 * 4.0", 10)]
    [InlineData("<< 10.0 / 2.0", 5)]
    [InlineData("<< -2.5", -2.5f)]
    [InlineData("<< 7.5 % 2.5", 0)]
    [InlineData("<< 10.0 % 3.0", 1)]
    public void Floats(string script, float expected)
    {
        // Act
        var results = Utils.Execute(script);

        // Assert
        results.AssertEqual(expected);
    }

    [Theory]
    [InlineData("<< (5 + 3) - 2", 6)]
    [InlineData("<< 5 + (3 - 2)", 6)]
    [InlineData("<< (10 - 5) + (8 - 3)", 10)]
    [InlineData("<< 100 - (50 + 25)", 25)]
    [InlineData("<< 2 + 3 * 4", 14)]
    [InlineData("<< (2 + 3) * 4", 20)]
    [InlineData("<< -5 + 3", -2)]
    [InlineData("<< 10 * -2", -20)]
    [InlineData("<< -2 * 3", -6)]
    public void Precedence(string script, int expected)
    {
        // Act
        var results = Utils.Execute(script);

        // Assert
        results.AssertEqual(expected);
    }

    [Fact]
    public void DivByZero()
    {
        var exception = Assert.Throws<RuntimeError>(() => Utils.Execute("<< 10 / 0"));
        Assert.Contains("Division by zero", exception.Message);
    }
}
