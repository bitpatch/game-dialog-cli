using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class IfElseTests
{
    [Fact]
    public void SimpleIf()
    {
        var source = """
        x = 5
        if x > 3
            << x
        """;

        var results = Utils.Execute(source);

        Assert.Equal(new object[] { 5 }, results);
    }

    [Fact]
    public void SimpleIfFalse()
    {
        var source = """
        x = 2
        if x > 3
            << x
        """;

        var results = Utils.Execute(source);

        Assert.Empty(results);
    }

    [Fact]
    public void IfElse()
    {
        var source = """
        x = 2
        if x > 3
            << x
        else
            << x + 1
        """;

        var results = Utils.Execute(source);

        Assert.Equal(new object[] { 3 }, results);
    }

    [Fact]
    public void IfElseIfElse()
    {
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

        var results = Utils.Execute(source);

        Assert.Equal(new object[] { 2 }, results);
    }

    [Fact]
    public void MultipleElseIf()
    {
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

        var results = Utils.Execute(source);

        Assert.Equal(new object[] { 2 }, results);
    }

    [Fact]
    public void NestedIf()
    {
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

        var results = Utils.Execute(source);

        Assert.Equal(new object[] { 15 }, results);
    }

    [Fact]
    public void IfWithMultipleStatements()
    {
        var source = """
        x = 3
        y = 4
        if x < y
            x = x + 1
            << x
            y = y + 1
            << y
        """;

        var results = Utils.Execute(source);

        Assert.Equal(new object[] { 4, 5 }, results);
    }

    [Fact]
    public void InvalidCondition()
    {
        var source = """
        x = 3
        if x + 1
            << x
        """;

        var ex = Assert.Throws<InvalidSyntaxException>(() => Utils.Execute(source));
        Assert.Equal(4, ex.Initial);
        Assert.Equal(9, ex.Final);
    }

    [Fact]
    public void NoThenBlock()
    {
        var source = """
        x = 3
        if x > 0
        << x
        """;

        var ex = Assert.Throws<InvalidSyntaxException>(() => Utils.Execute(source));
        Assert.Equal(2, ex.Line);
        Assert.Equal(9, ex.Initial);
        Assert.Equal(10, ex.Final);
    }

    [Fact]
    public void NoElseBlock()
    {
        var source = """
        x = 3
        if x > 0
            << x
        else
        << x + 1
        """;

        var ex = Assert.Throws<InvalidSyntaxException>(() => Utils.Execute(source));
        Assert.Equal(4, ex.Line);
        Assert.Equal(5, ex.Initial);
        Assert.Equal(6, ex.Final);
    }

    [Fact]
    public void NoElseIfBlock()
    {
        var source = """
        x = 3
        if x > 0
            << x
        else if x == 0
        << 0
        """;

        var ex = Assert.Throws<InvalidSyntaxException>(() => Utils.Execute(source));
        Assert.Equal(4, ex.Line);
        Assert.Equal(15, ex.Initial);
        Assert.Equal(16, ex.Final);
    }

    [Fact]
    public void ElseIfWithInvalidCondition()
    {
        var source = """
        x = 3
        if x > 5
            << 1
        else if x + 1
            << 2
        """;

        var ex = Assert.Throws<InvalidSyntaxException>(() => Utils.Execute(source));
        Assert.Equal(4, ex.Line);
        Assert.Equal(9, ex.Initial);
        Assert.Equal(14, ex.Final);
    }

    [Fact]
    public void AllConditionsFalseNoElse()
    {
        var source = """
        x = 100
        if x < 10
            << 1
        else if x < 50
            << 2
        """;

        var results = Utils.Execute(source);

        Assert.Empty(results);
    }
}
