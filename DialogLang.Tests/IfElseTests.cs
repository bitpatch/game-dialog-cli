using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class IfElseTests
{
    [Fact]
    public void SimpleIf()
    {
        // Arrange
        var source = """
        x = 5
        if x > 3
            << x
        """;

        // Act
        var results = Utils.Execute(source);

        // Assert
        results.AssertEqual(5);
    }

    [Fact]
    public void SimpleIfFalse()
    {
        // Arrange
        var source = """
        x = 2
        if x > 3
            << x
        """;

        // Act
        var results = Utils.Execute(source);

        // Assert
        Assert.Empty(results);
    }

    [Fact]
    public void IfElse()
    {
        // Arrange
        var source = """
        x = 2
        if x > 3
            << x
        else
            << x + 1
        """;

        // Act
        var results = Utils.Execute(source);

        // Assert
        results.AssertEqual(3);
    }

    [Fact]
    public void IfElseIfElse()
    {
        // Arrange
        var source = """
        x = 5
        y = 5
        if x < y
            << 1
        else if x == y
            << 2
        else
            << 3
        """;

        // Act
        var results = Utils.Execute(source);

        // Assert
        results.AssertEqual(2);
    }

    [Fact]
    public void MultipleElseIf()
    {
        // Arrange
        var source = """
        x = 15
        if x < 10
            << 1
        else if x < 20
            << 2
        else if x < 30
            << 3
        else
            << 4
        """;

        // Act
        var results = Utils.Execute(source);

        // Assert
        results.AssertEqual(2);
    }

    [Fact]
    public void NestedIf()
    {
        // Arrange
        var source = """
        x = 5
        y = 10
        if x > 0
            if y > 5
                << x + y
            else
                << x
        else
            << 0
        """;

        // Act
        var results = Utils.Execute(source);

        // Assert
        results.AssertEqual(15);
    }

    [Fact]
    public void IfWithMultipleStatements()
    {
        // Arrange
        var source = """
        x = 3
        y = 4
        if x < y
            x = x + 1
            << x
            y = y + 1
            << y
        """;

        // Act
        var results = Utils.Execute(source);

        // Assert
        results.AssertEqual(4, 5);
    }

    [Fact]
    public void InvalidCondition()
    {
        // Arrange
        var source = """
        x = 3
        if x + 1
            << x
        """;

        // Act
        var ex = Assert.Throws<SyntaxError>(() => Utils.Execute(source));

        // Assert
        ex.AssertLocation(2, 4, 9);
    }

    [Fact]
    public void NoThenBlock()
    {
        // Arrange
        var source = """
        x = 3
        if x > 0
        << x
        """;

        // Act
        var ex = Assert.Throws<SyntaxError>(() => Utils.Execute(source));

        // Assert
        ex.AssertLocation(2, 9, 10);
    }

    [Fact]
    public void NoElseBlock()
    {
        // Arrange
        var source = """
        x = 3
        if x > 0
            << x
        else
        << x + 1
        """;

        // Act
        var ex = Assert.Throws<SyntaxError>(() => Utils.Execute(source));

        // Assert
        ex.AssertLocation(4, 5, 6);
    }

    [Fact]
    public void NoElseIfBlock()
    {
        // Arrange
        var source = """
        x = 3
        if x > 0
            << x
        else if x == 0
        << 0
        """;

        // Act
        var ex = Assert.Throws<SyntaxError>(() => Utils.Execute(source));

        // Assert
        ex.AssertLocation(4, 15, 16);
    }

    [Fact]
    public void ElseIfWithInvalidCondition()
    {
        // Arrange
        var source = """
        x = 3
        if x > 5
            << 1
        else if x + 1
            << 2
        """;

        // Act
        var ex = Assert.Throws<SyntaxError>(() => Utils.Execute(source));

        // Assert
        ex.AssertLocation(4, 9, 14);
    }

    [Fact]
    public void AllConditionsFalseNoElse()
    {
        // Arrange
        var source = """
        x = 100
        if x < 10
            << 1
        else if x < 50
            << 2
        """;

        // Act
        var results = Utils.Execute(source);

        // Assert
        Assert.Empty(results);
    }
}
