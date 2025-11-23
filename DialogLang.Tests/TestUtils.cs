using System.Diagnostics;
using BitPatch.DialogLang;

namespace DialogLang.Tests;

internal static class Utils
{
    /// <summary>
    /// Executes the given script and returns the list of runtime items.
    /// </summary>
    /// <param name="script">The script to execute.</param>
    /// <returns>A list of runtime items resulting from the script execution.</returns>
    public static List<RuntimeItem> Execute(string script)
    {
        var dialog = new Dialog();
        return [.. dialog.RunInline(script)];
    }

    /// <summary>
    /// Parses the given source code into a list of AST statements.
    /// </summary>
    public static List<BitPatch.DialogLang.Ast.Statement> Parse(this string source)
    {
        using var lexer = new Lexer(Source.Inline(source));
        var parser = new Parser(lexer.Tokenize());
        return [.. parser.Parse()];
    }

    /// <summary>
    /// Tokenizes the given source code into a list of tokens.
    /// </summary>
    public static List<Token> Tokenize(this string source)
    {
        using var lexer = new Lexer(Source.Inline(source));
        return [.. lexer.Tokenize()];
    }

    /// <summary>
    /// Asserts that the actual list of runtime items matches the expected single integer value.
    /// </summary>
    [StackTraceHidden]
    public static void AssertEqual(this List<RuntimeItem> actual, int expected)
    {
        Assert.Single(actual);
        var integer = Assert.IsType<Integer>(actual[0]);
        Assert.Equal(expected, integer.Value);
    }

    /// <summary>
    /// Asserts that the actual list of runtime items matches the expected single float value.
    /// </summary>
    [StackTraceHidden]
    public static void AssertEqual(this List<RuntimeItem> actual, float expected)
    {
        Assert.Single(actual);
        var number = Assert.IsType<Float>(actual[0]);
        Assert.Equal(expected, number.FloatValue, precision: 5);
    }

    /// <summary>
    /// Asserts that the actual list of runtime items matches the expected single string value.
    /// </summary>
    [StackTraceHidden]
    public static void AssertEqual(this List<RuntimeItem> actual, string expected)
    {

        Assert.Single(actual);
        var str = Assert.IsType<BitPatch.DialogLang.String>(actual[0]);
        Assert.Equal(expected, str.Value);
    }

    /// <summary>
    /// Asserts that the actual list of runtime items matches the expected single boolean value.
    /// </summary>
    [StackTraceHidden]
    public static void AssertEqual(this List<RuntimeItem> actual, bool expected)
    {

        Assert.Single(actual);
        var boolean = Assert.IsType<BitPatch.DialogLang.Boolean>(actual[0]);
        Assert.Equal(expected, boolean.Value);
    }

    /// <summary>
    /// Asserts that the actual list of runtime items represents a true boolean value.
    /// </summary>
    [StackTraceHidden]
    public static void AssertFalse(this List<RuntimeItem> actual)
    {
        actual.AssertEqual(false);
    }

    /// <summary>
    /// Asserts that the actual list of runtime items represents a true boolean value.
    /// </summary>
    [StackTraceHidden]
    public static void AssertTrue(this List<RuntimeItem> actual)
    {
        actual.AssertEqual(true);
    }

    /// <summary>
    /// Asserts that the actual list of runtime items matches the expected values.
    /// </summary>
    [StackTraceHidden]
    public static void AssertEqual(this List<RuntimeItem> actual, params object[] expected)
    {
        Assert.Equal(expected.Length, actual.Count);

        for (int i = 0; i < expected.Length; i++)
        {
            if (actual[i] is Value value)
            {
                Assert.Equal(expected[i], value.GetValue());
            }
            else
            {
                Assert.Fail($"Unexpected value type: {expected[i].GetType().FullName}");
            }
        }
    }

    /// <summary>
    /// Asserts that the given ScriptException has the expected location.
    /// </summary>
    [StackTraceHidden]
    public static void AssertLocation(this ScriptException ex, int line, int initial, int final)
    {
        Assert.Equal([line, initial, final], [ex.Line, ex.Initial, ex.Final]);
    }

    /// <summary>
    /// Recursively adds the types of the statement and its inner statements to the result list.
    /// </summary>
    public static List<Type> AddTypesTo(this BitPatch.DialogLang.Ast.Statement statement, List<Type> result)
    {
        result.Add(statement.GetType());

        return statement switch
        {
            BitPatch.DialogLang.Ast.Block block => block.Statements.AddTypesTo(result),
            BitPatch.DialogLang.Ast.While whileLoop => whileLoop.Body.AddTypesTo(result),
            _ => result
        };
    }

    /// <summary>
    /// Recursively adds the types of the statements and their inner statements to the result list.
    /// </summary>
    public static List<Type> AddTypesTo(this IReadOnlyList<BitPatch.DialogLang.Ast.Statement> statements, List<Type> result)
    {
        foreach (var statement in statements)
        {
            statement.AddTypesTo(result);
        }

        return result;
    }
}