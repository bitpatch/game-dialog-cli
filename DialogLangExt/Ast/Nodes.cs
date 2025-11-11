using System.Collections.Generic;

namespace BitPatch.DialogLang.Ast
{
    /// <summary>
    /// Base class for all AST nodes
    /// </summary>
    internal abstract record Node;

    /// <summary>
    /// Base class for all statements (executed, do not return values)
    /// </summary>
    internal abstract record Statement : Node;

    /// <summary>
    /// Base class for all expressions (evaluated, return values)
    /// </summary>
    internal abstract record Expression : Node;

    /// <summary>
    /// Root node representing the entire program
    /// </summary>
    internal record Program(List<Node> Statements) : Node
    {
        public Program() : this(new List<Node>())
        {
        }
    }

    /// <summary>
    /// Base class for all value (literal) nodes
    /// </summary>
    internal abstract record Value : Expression;

    /// <summary>
    /// Node representing an integer number literal
    /// </summary>
    internal record Number(int Value) : Value;

    /// <summary>
    /// Node representing a variable reference
    /// </summary>
    internal record Variable(string Name) : Expression;

    /// <summary>
    /// Node representing an assignment statement
    /// </summary>
    internal record Assign(string VariableName, Expression Value) : Statement;
}
