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
}