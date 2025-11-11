using System.Collections.Generic;

namespace BitPatch.DialogLang.Ast
{
    /// <summary>
    /// Base class for all AST nodes
    /// </summary>
    internal abstract record Node(int Line, int Column);

    /// <summary>
    /// Base class for all statements (executed, do not return values)
    /// </summary>
    internal abstract record Statement(int Line, int Column) : Node(Line, Column);

    /// <summary>
    /// Base class for all expressions (evaluated, return values)
    /// </summary>
    internal abstract record Expression(int Line, int Column) : Node(Line, Column);

    /// <summary>
    /// Root node representing the entire program
    /// </summary>
    internal record Program(List<Node> Statements, int Line, int Column) : Node(Line, Column);

    /// <summary>
    /// Base class for all value (literal) nodes
    /// </summary>
    internal abstract record Value(int Line, int Column) : Expression(Line, Column);

    /// <summary>
    /// Node representing an integer number literal
    /// </summary>
    internal record Number(int Value, int Line, int Column) : Value(Line, Column);

    /// <summary>
    /// Node representing a string literal
    /// </summary>
    internal record String(string Value, int Line, int Column) : Value(Line, Column);

    /// <summary>
    /// Node representing a variable reference
    /// </summary>
    internal record Variable(string Name, int Line, int Column) : Expression(Line, Column);

    /// <summary>
    /// Node representing an identifier (variable name)
    /// </summary>
    internal record Identifier(string Name, int Line, int Column) : Node(Line, Column);

    /// <summary>
    /// Node representing an assignment statement
    /// </summary>
    internal record Assign(Identifier Identifier, Expression Expression, int Line, int Column) : Statement(Line, Column);
}
