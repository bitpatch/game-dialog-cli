using System.Collections.Generic;

namespace BitPatch.DialogLang.Ast
{
    /// <summary>
    /// Base class for all AST nodes
    /// </summary>
    internal abstract record Node;

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
    /// Node representing an integer number literal
    /// </summary>
    internal record Number(int Value) : Node;

    /// <summary>
    /// Node representing a variable reference
    /// </summary>
    internal record Variable(string Name) : Node;

    /// <summary>
    /// Node representing an assignment statement
    /// </summary>
    internal record Assign(string VariableName, Node Value) : Node;
}
