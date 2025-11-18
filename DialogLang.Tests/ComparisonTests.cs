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
    [InlineData("<< 5 > 3", true)]
    [InlineData("<< 3 > 5", false)]
    [InlineData("<< 5 > 5", false)]
    [InlineData("<< 5.5 > 3.2", true)]
    [InlineData("<< 2.1 > 4.8", false)]
    [InlineData("<< 10 > 5.5", true)]
    [InlineData("<< 3.5 > 7", false)]
    public void GreaterThan(string script, bool expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Theory]
    [InlineData("<< 3 < 5", true)]
    [InlineData("<< 5 < 3", false)]
    [InlineData("<< 5 < 5", false)]
    [InlineData("<< 3.2 < 5.5", true)]
    [InlineData("<< 4.8 < 2.1", false)]
    [InlineData("<< 5.5 < 10", true)]
    [InlineData("<< 7 < 3.5", false)]
    public void LessThan(string script, bool expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Theory]
    [InlineData("<< 5 >= 3", true)]
    [InlineData("<< 5 >= 5", true)]
    [InlineData("<< 3 >= 5", false)]
    [InlineData("<< 5.5 >= 3.2", true)]
    [InlineData("<< 4.5 >= 4.5", true)]
    [InlineData("<< 2.1 >= 4.8", false)]
    public void GreaterOrEqual(string script, bool expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Theory]
    [InlineData("<< 3 <= 5", true)]
    [InlineData("<< 5 <= 5", true)]
    [InlineData("<< 5 <= 3", false)]
    [InlineData("<< 3.2 <= 5.5", true)]
    [InlineData("<< 4.5 <= 4.5", true)]
    [InlineData("<< 4.8 <= 2.1", false)]
    public void LessOrEqual(string script, bool expected)
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
    public void Equal(string script, bool expected)
    {
        // Act
        var results = ExecuteScript(script);

        // Assert
        Assert.Equal(new object[] { expected }, results);
    }

    [Theory]
    [InlineData("<< 5 != 3", true)]
    [InlineData("<< 5 != 5", false)]
    [InlineData("<< 3.14 != 2.71", true)]
    [InlineData("<< 2.5 != 2.5", false)]
    [InlineData("<< 5 != 5.0", false)]
    [InlineData("<< true != false", true)]
    [InlineData("<< true != true", false)]
    [InlineData("<< \"hello\" != \"world\"", true)]
    [InlineData("<< \"hello\" != \"hello\"", false)]
    public void NotEqual(string script, bool expected)
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
    [InlineData("<< 5 > \"hello\"", 4)]
    [InlineData("<< \"test\" < 10", 4)]
    [InlineData("<< true >= 5", 4)]
    [InlineData("<< 3.14 <= false", 4)]
    [InlineData("<< \"abc\" > \"def\"", 4)]
    [InlineData("<< \"hello\" < 5", 4)]
    [InlineData("<< 10 >= \"world\"", 4)]
    [InlineData("<< false <= 3.14", 4)]
    [InlineData("<< true > false", 4)]
    [InlineData("<< \"test\" >= 100", 4)]
    public void TypeMismatchInNumericComparisons(string script, int expectedColumn)
    {
        var exception = Assert.Throws<TypeMismatchException>(() => ExecuteScript(script));
        Assert.Equal(expectedColumn, exception.Column);
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
        var exception = Assert.Throws<TypeMismatchException>(() => ExecuteScript("""
            a = 5
            b = "hello"
            << a > b
            """));
        
        Assert.Equal(4, exception.Column);
    }

    [Fact]
    public void ComplexTypeErrors()
    {
        // String vs Number
        Assert.Throws<TypeMismatchException>(() => ExecuteScript("""
            name = "Alice"
            age = 25
            << name < age
            """));

        // Boolean vs Number
        Assert.Throws<TypeMismatchException>(() => ExecuteScript("""
            flag = true
            value = 10
            << flag >= value
            """));

        // String vs Boolean
        Assert.Throws<TypeMismatchException>(() => ExecuteScript("""
            text = "true"
            bool = true
            << text > bool
            """));
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
