using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class BooleanTests
{
    private static List<object> ExecuteScript(string script)
    {
        var dialog = new Dialog();
        return dialog.Execute(script).ToList();
    }

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
        var results = ExecuteScript(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Fact]
    public void VariablesWithBooleans()
    {
        var results = ExecuteScript("""
            x = true
            << x
            y = false
            << y
            """);

        Assert.Equal(new object[] { true, false }, results);
    }

    [Fact]
    public void ComplexExpression()
    {
        var results = ExecuteScript("""
            a = true
            b = false
            c = true
            << (a or b) and not (b xor c)
            """);

        Assert.Equal(new object[] { false }, results);
    }

    [Fact]
    public void MultipleOutputs()
    {
        var results = ExecuteScript("""
            << true and false
            << false or true
            << not false
            """);

        Assert.Equal(new object[] { false, true, true }, results);
    }

    [Fact]
    public void VariableReassignment()
    {
        var results = ExecuteScript("""
            x = true
            << x
            x = false
            << x
            x = not x
            << x
            """);

        Assert.Equal(new object[] { true, false, true }, results);
    }

    [Theory]
    [InlineData("<< and true")]
    [InlineData("<< true and")]
    [InlineData("<< (true and false")]
    [InlineData("<< true and false)")]
    public void InvalidSyntax(string script)
    {
        Assert.Throws<InvalidSyntaxException>(() => ExecuteScript(script));
    }

    [Theory]
    [InlineData("<< true and 5")]
    [InlineData("<< 42 and false")]
    [InlineData("<< false or \"hello\"")]
    [InlineData("<< true xor 10")]
    [InlineData("<< not \"test\"")]
    [InlineData("<< not 123")]
    public void TypeMismatch(string script)
    {
        Assert.Throws<ScriptException>(() => ExecuteScript(script));
    }
}
