using System.Collections.Generic;

namespace BitPatch.DialogLang
{
    /// <summary>
    /// Base class for all AST nodes
    /// </summary>
    internal abstract record AstNode;

    /// <summary>
    /// Root node representing the entire program
    /// </summary>
    internal record ProgramNode(List<AstNode> Statements) : AstNode
    {
        public ProgramNode() : this(new List<AstNode>())
        {
        }
    }

    /// <summary>
    /// Node representing an integer number literal
    /// </summary>
    internal record NumberNode(int Value) : AstNode;

    /// <summary>
    /// Node representing a variable reference
    /// </summary>
    internal record VariableNode(string Name) : AstNode;

    /// <summary>
    /// Node representing an assignment statement
    /// </summary>
    internal record AssignNode(string VariableName, AstNode Value) : AstNode;
}
