using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class BooleanTests
{
    [Theory]
    [InlineData("<< true", true)]
    [InlineData("<< false", false)]
    [InlineData("<< true and true", true)]
    [InlineData("<< true and false", false)]
    [InlineData("<< false and true", false)]
    [InlineData("<< false and false", false)]
    [InlineData("<< true or true", true)]
    [InlineData("<< true or false", true)]
    [InlineData("<< false or true", true)]
    [InlineData("<< false or false", false)]
    [InlineData("<< true xor true", false)]
    [InlineData("<< true xor false", true)]
    [InlineData("<< false xor true", true)]
    [InlineData("<< false xor false", false)]
    [InlineData("<< not true", false)]
    [InlineData("<< not false", true)]
    [InlineData("<< not not true", true)]
    [InlineData("<< true and false or true", true)]
    [InlineData("<< false or true and false", false)]
    [InlineData("<< not false and true", true)]
    [InlineData("<< (true or false) and false", false)]
    [InlineData("<< true and (false or true)", true)]
    [InlineData("<< ((true and false) or (false and true))", false)]
    [InlineData("<< true xor true xor false", false)]
    public void BooleanLiterals(string script, bool expected)
    {
        // Act
        var results = Utils.Execute(script);

        // Assert
        results.AssertEqual(expected);
    }

    [Theory]
    [InlineData("<< and true")]
    [InlineData("<< true and")]
    [InlineData("<< (true and false")]
    [InlineData("<< true and false)")]
    public void InvalidSyntax(string script)
    {
        Assert.Throws<SyntaxError>(() => Utils.Execute(script));
    }

    [Theory]
    [InlineData("<< false or (5 + 1)", 13, 20)]
    [InlineData("<< false or \"hello\"", 13, 20)]
    [InlineData("<< not (true or 123)", 17, 20)]
    [InlineData("<< true xor 10.3", 13, 17)]
    [InlineData("<< true and 5", 13, 14)]
    [InlineData("<< 42 and \"hi\"", 4, 6)]
    [InlineData("<< not \"test\"", 8, 14)]
    public void CannotBeBoolean(string script, int initial, int final)
    {
        // Act
        var ex = Assert.Throws<SyntaxError>(() => Utils.Execute(script));

        // Assert
        ex.AssertLocation(1, initial, final);
    }
}
