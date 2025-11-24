using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class ComparisonTests
{
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
        var results = Utils.Execute(script);

        // Assert
        results.AssertEqual(expected);
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
        var results = Utils.Execute(script);

        // Assert
        results.AssertEqual(expected);
    }

    [Fact]
    public void VariablesWithComparisons()
    {
        var results = Utils.Execute("""
            a = 5
            b = 3
            << a > b
            << a < b
            << a >= 5
            << b <= 3
            << a == 5
            << a != b
            """);

        results.AssertEqual([true, false, true, true, true, true]);
    }



    [Theory]
    [InlineData("<< 5 > \"hello\"", 8, 15)]
    [InlineData("<< \"test\" < 10", 4, 10)]
    [InlineData("<< true >= \"5\"", 4, 15)]
    public void CannotCompare(string script, int initial, int final)
    {
        // Act
        var ex = Assert.Throws<RuntimeError>(() => Utils.Execute(script));

        // Assert
        ex.AssertLocation(1, initial, final);
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
        var results = Utils.Execute(script);

        // Assert - different types are never equal
        results.AssertFalse();
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
        var results = Utils.Execute(script);

        // Assert - different types are always not equal
        results.AssertTrue();
    }
}
