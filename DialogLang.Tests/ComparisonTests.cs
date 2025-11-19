using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class ComparisonTests
{
    private static List<object> ExecuteScript(string script)
    {
        var dialog = new Dialog();
        return [.. dialog.Execute(script)];
    }

    [Theory]
    [InlineData("<< 5 > 0", true)]
    [InlineData("<< 3 < 2.99999", false)]
    [InlineData("<< 5 > 5.0", false)]
    [InlineData("<< 5 >= 5.0", true)]
    [InlineData("<< 5.5 > 5", true)]
    [InlineData("<< 6 > 5.9999", true)]
    [InlineData("<< 0.01 > 0.001", true)]
    public void Comparison(string script, bool expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Theory]
    [InlineData("<< 5 == 5", true)]
    [InlineData("<< 5 == 5.0", true)]
    [InlineData("<< 5 == 3", false)]
    [InlineData("<< 3.14 == 3.14", true)]
    [InlineData("<< 2.5 == 3.5", false)]
    [InlineData("<< 3.0 == 3", true)]
    [InlineData("<< true == true", true)]
    [InlineData("<< true == false", false)]
    [InlineData("<< false == false", true)]
    [InlineData("<< \"hello\" == \"hello\"", true)]
    [InlineData("<< \"hello\" == \"world\"", false)]
    [InlineData("<< 5 != 3", true)]
    [InlineData("<< 5 != 5", false)]
    [InlineData("<< 3.14 != 2.71", true)]
    [InlineData("<< 2.5 != 2.5", false)]
    [InlineData("<< 5 != 5.0", false)]
    [InlineData("<< true != false", true)]
    [InlineData("<< true != true", false)]
    [InlineData("<< \"hello\" != \"world\"", true)]
    [InlineData("<< \"hello\" != \"hello\"", false)]
    public void Equal(string script, bool expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Fact]
    public void VariablesWithComparisons()
    {
        var results = ExecuteScript("""
            a = 5
            b = 3
            << a > b
            << a < b
            << a >= 5
            << b <= 3
            << a == 5
            << a != b
            """);

        Assert.Equal(new object[] { true, false, true, true, true, true }, results);
    }



    [Theory]
    [InlineData("<< 5 > \"hello\"", 8, 15)]
    [InlineData("<< \"test\" < 10", 4, 10)]
    [InlineData("<< true >= \"5\"", 4, 15)]
    public void CannotCompare(string script, int initial, int final)
    {
        // Act
        var ex = Assert.Throws<ScriptException>(() => ExecuteScript(script));
        Assert.Equal(initial, ex.Initial);
        Assert.Equal(final, ex.Final);
    }

    [Theory]
    [InlineData("<< 5 == \"5\"")]
    [InlineData("<< \"hello\" == 42")]
    [InlineData("<< true == 1")]
    [InlineData("<< false == 0")]
    [InlineData("<< 3.14 == \"3.14\"")]
    [InlineData("<< \"true\" == true")]
    public void EqualityWithDifferentTypes(string script)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert - different types are never equal
        Assert.Equal(new object[] { false }, results);
    }

    [Theory]
    [InlineData("<< 5 != \"5\"")]
    [InlineData("<< \"hello\" != 42")]
    [InlineData("<< true != 1")]
    [InlineData("<< false != 0")]
    [InlineData("<< 3.14 != \"3.14\"")]
    public void InequalityWithDifferentTypes(string script)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert - different types are always not equal
        Assert.Equal(new object[] { true }, results);
    }
}
