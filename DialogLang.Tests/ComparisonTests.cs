using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class ComparisonTests
{
    private static List<object> ExecuteScript(string script)
    {
        var dialog = new Dialog();
        return dialog.Execute(script).ToList();
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

    [Fact]
    public void FloatComparisons()
    {
        var results = ExecuteScript("""
            x = 3.14
            y = 2.71
            << x > y
            << x < y
            << x == 3.14
            << y != 3.14
            """);

        Assert.Equal(new object[] { true, false, true, true }, results);
    }

    [Fact]
    public void MixedIntegerAndFloat()
    {
        var results = ExecuteScript("""
            a = 5
            b = 3.5
            << a > b
            << b < a
            << a >= 5.0
            << b <= 4
            """).ToList();

        Assert.Equal(new object[] { true, true, true, true }, results);
    }

    [Fact]
    public void ComparisonWithLogicalOperators()
    {
        var results = ExecuteScript("""
            << (5 > 3) and (2 < 4)
            << (5 < 3) or (2 == 2)
            << not (5 == 3)
            << (5 >= 5) and (3 <= 3)
            """);

        Assert.Equal(new object[] { true, true, true, true }, results);
    }

    [Fact]
    public void ChainedComparisons()
    {
        var results = ExecuteScript("""
            a = 5
            b = 10
            c = 15
            << a < b and b < c
            << a == 5 and b == 10 and c == 15
            << a < b or b > c
            """);

        Assert.Equal(new object[] { true, true, true }, results);
    }

    [Theory]
    [InlineData("<< 5 > \"hello\"", 8, 15)]
    [InlineData("<< \"test\" < 10", 4, 10)]
    [InlineData("<< true >= \"5\"", 4, 15)]
    public void CannotCompare(string script, int startColumn, int endColumn)
    {
        // Act
        var exception = Assert.Throws<ScriptException>(() => ExecuteScript(script));
        Assert.Equal(startColumn, exception.StartColumn);
        Assert.Equal(endColumn, exception.EndColumn);
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

    [Fact]
    public void TypeMismatchWithVariables()
    {
        var ex = Assert.Throws<ScriptException>(() => ExecuteScript("""
            a = 5
            b = "hello"
            << a > b
            """));

        Assert.Equal(8, ex.StartColumn);
        Assert.Equal(9, ex.EndColumn);
    }

    [Fact]
    public void ComplexTypeErrors()
    {
        // String vs Number
        var ex = Assert.Throws<ScriptException>(() => ExecuteScript("""
            name = "Alice"
            age = 25
            << name < age
            """));

        Assert.Equal(4, ex.StartColumn);
        Assert.Equal(8, ex.EndColumn);

        // Boolean vs Number
        ex = Assert.Throws<ScriptException>(() => ExecuteScript("""
            flag = true
            value = 10
            << value >= flag
            """));

        Assert.Equal(13, ex.StartColumn);
        Assert.Equal(17, ex.EndColumn);

        // String vs Boolean
        ex = Assert.Throws<ScriptException>(() => ExecuteScript("""
            text = "true"
            bool = true
            << text > bool
            """));

        Assert.Equal(4, ex.StartColumn);
        Assert.Equal(15, ex.EndColumn);
    }

    [Fact]
    public void ComparisonOperatorPrecedence()
    {
        var results = ExecuteScript("""
            << 5 > 3 and 2 < 4
            << 5 == 5 or 3 > 10
            << not (5 < 3)
            << 5 > 3 xor 2 > 1
            """);

        Assert.Equal(new object[] { true, true, true, false }, results);
    }

    [Fact]
    public void ParenthesizedComparisons()
    {
        var results = ExecuteScript("""
            << (5 > 3)
            << ((10 < 20) and (5 >= 5))
            << (not (3 == 5))
            """);

        Assert.Equal(new object[] { true, true, true }, results);
    }
}
