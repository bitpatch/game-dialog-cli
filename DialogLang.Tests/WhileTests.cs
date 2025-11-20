using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class WhileTests
{
    [Fact]
    public void Simple()
    {
        var source = """
        x = 3
        while x > 0
            << x
            x = x - 1
        """;

        var results = Utils.Execute(source);

        Assert.Equal(new object[] { 3, 2, 1 }, results);
    }

    [Fact]
    public void InvalidCondition()
    {
        var source = """
        x = 3
        while x - 1
            << x
            x = x - 1
        """;

        var ex = Assert.Throws<InvalidSyntaxException>(() => Utils.Execute(source));
        Assert.Equal(7, ex.Initial);
        Assert.Equal(12, ex.Final);
    }

    [Fact]
    public void NoBody()
    {
        var source = """
        x = 3
        while x > 0
        << x
        x = x - 1
        """;

        var ex = Assert.Throws<InvalidSyntaxException>(() => Utils.Execute(source));
        Assert.Equal(2, ex.Line);
        Assert.Equal(12, ex.Initial);
        Assert.Equal(13, ex.Final);
    }

    [Fact]
    public void CannotCompare()
    {
        var source = """
        a = true
        while a > 0
            << a
            a = false
        """;

        var ex = Assert.Throws<ScriptException>(() => Utils.Execute(source));
        Assert.Equal(2, ex.Line);
        Assert.Equal(7, ex.Initial);
        Assert.Equal(8, ex.Final);
    }

    [Fact]
    public void ExpectedBoolean()
    {
        var source = """
        value = "Hello"
        while value
            << a
            a = false
        """;

        var ex = Assert.Throws<ScriptException>(() => Utils.Execute(source));
        Assert.Equal(2, ex.Line);
        Assert.Equal(7, ex.Initial);
        Assert.Equal(12, ex.Final);
    }
}