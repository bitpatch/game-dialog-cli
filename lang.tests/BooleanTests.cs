using BitPatch.DialogLang;

namespace Lang.Tests;

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
}
