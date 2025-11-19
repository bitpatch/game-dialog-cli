using BitPatch.DialogLang.Ast;

namespace DialogLang.Tests;

public class Parser
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

        var statements = Parse(source);
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

        var result = new List<Type>();

        foreach (var statement in statements)
        {
            AddStatementType(statement);
        }

        Assert.Equal(expected, result);

        void AddStatementType(Statement statement)
        {
            result.Add(statement.GetType());

            if (statement is Block block)
            {
                foreach (var innerStmt in block.Statements)
                {
                    AddStatementType(innerStmt);
                }
            }
        }
    }
}
