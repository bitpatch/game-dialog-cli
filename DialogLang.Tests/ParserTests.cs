using BitPatch.DialogLang.Ast;

namespace DialogLang.Tests;

public class ParserTests
{
    private static List<Statement> Parse(string source)
    {
        using var reader = new StringReader(source);
        var lexer = new BitPatch.DialogLang.Lexer(reader);
        var parser = new BitPatch.DialogLang.Parser(lexer.Tokenize());
        return [.. parser.Parse()];
    }

    [Fact]
    public void Block()
    {
        var source = """
        x = 1
            y = 2
            << y
                << y + 1
        << x
        """;

        var expected = new[]
        {
            typeof(Assign), // x = 1
            typeof(Block),  // block:
            typeof(Assign), //   y = 2
            typeof(Output), //   << y
            typeof(Block),  //     block:
            typeof(Output), //     << y + 1
            typeof(Output)  // << x
        };

        var statements = Parse(source);
        var result = new List<Type>();

        foreach (var statement in statements)
        {
            AddStatementType(statement, result);
        }

        Assert.Equal(expected, result);
    }

    [Fact]
    public void WhileLoop()
    {
        var source = """
        x = 3
        while x > 0
            << x
            x = x - 1
        """;

        var expected = new[]
        {
            typeof(Assign), // x = 3
            typeof(While),  // while x > 0
            typeof(Block),  // block:
            typeof(Output), //   << x
            typeof(Assign)  //   x = x - 1
        };

        var statements = Parse(source);
        var result = new List<Type>();

        foreach (var statement in statements)
        {
            AddStatementType(statement, result);
        }

        Assert.Equal(expected, result);
    }

    private static void AddStatementType(Statement statement, List<Type> result)
    {
        result.Add(statement.GetType());

        switch (statement)
        {
            case Block block:
                foreach (var innerStmt in block.Statements)
                {
                    AddStatementType(innerStmt, result);
                }
                break;
            case While whileLoop:
                AddStatementType(whileLoop.Body, result);
                break;
        }
    }
}
