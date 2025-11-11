using System.Collections.Generic;

namespace BitPatch.DialogLang
{
    /// <summary>
    /// Base class for all AST nodes
    /// </summary>
    internal abstract class AstNode
    {
    }

    /// <summary>
    /// Root node representing the entire program
    /// </summary>
    internal class ProgramNode : AstNode
    {
        public List<AstNode> Statements { get; }

        public ProgramNode()
        {
            Statements = new List<AstNode>();
        }
    }

    /// <summary>
    /// Node representing an integer number literal
    /// </summary>
    internal class NumberNode : AstNode
    {
        public int Value { get; }

        public NumberNode(int value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Node representing a variable reference
    /// </summary>
    internal class VariableNode : AstNode
    {
        public string Name { get; }

        public VariableNode(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Node representing an assignment statement
    /// </summary>
    internal class AssignNode : AstNode
    {
        public string VariableName { get; }
        public AstNode Value { get; }

        public AssignNode(string variableName, AstNode value)
        {
            VariableName = variableName;
            Value = value;
        }
    }
}
