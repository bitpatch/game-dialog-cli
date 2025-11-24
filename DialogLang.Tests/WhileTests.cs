using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class WhileTests
{
    [Fact]
    public void Simple()
    {
        // Arrange
        var source = """
        x = 3
        while x > 0
            << x
            x = x - 1
        """;

        // Act
        var results = Utils.Execute(source);

        // Assert
        results.AssertEqual(3, 2, 1);
    }

    [Fact]
    public void InvalidCondition()
    {
        // Arrange
        var source = """
        x = 3
        while x - 1
            << x
            x = x - 1
        """;

        // Act
        var ex = Assert.Throws<RuntimeError>(() => Utils.Execute(source));

        // Assert
        ex.AssertLocation(2, 7, 12);
    }

    [Fact]
    public void NoBody()
    {
        // Arrange
        var source = """
        x = 3
        while x > 0
        << x
        x = x - 1
        """;

        // Act
        var ex = Assert.Throws<SyntaxError>(() => Utils.Execute(source));

        // Assert
        ex.AssertLocation(2, 12, 13);
    }

    [Fact]
    public void CannotCompare()
    {
        // Arrange
        var source = """
        a = true
        while a > 0
            << a
            a = false
        """;

        // Act
        var ex = Assert.Throws<RuntimeError>(() => Utils.Execute(source));

        // Assert
        ex.AssertLocation(2, 7, 8);
    }

    [Fact]
    public void ExpectedBoolean()
    {
        // Arrange
        var source = """
        value = "Hello"
        while value
            << a
            a = false
        """;

        // Act
        var ex = Assert.Throws<RuntimeError>(() => Utils.Execute(source));

        // Assert
        ex.AssertLocation(2, 7, 12);
    }

    [Fact]
    public void InfinitBool()
    {
        // Arrange
        var source = """
        a = 0
        while true
            a = a + 1
            << a
        """;    

        // Act
        var ex = Assert.Throws<RuntimeError>(() => Utils.Execute(source));

        // Assert
        ex.AssertLocation(2, 1, 11);
    }
}