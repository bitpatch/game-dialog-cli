using System.Collections.Generic;

namespace BitPatch.DialogLang.Ast
{
    /// <summary>
    /// Base class for all AST nodes
    /// </summary>
    internal abstract record Node(TokenPosition Position)
    {
        public int Line => Position.Line;
        public int Column => Position.Column;
    }

    /// <summary>
    /// Base class for all statements (executed, do not return values)
    /// </summary>
    internal abstract record Statement(TokenPosition Position) : Node(Position);

    /// <summary>
    /// Base class for all expressions (evaluated, return values)
    /// </summary>
    internal abstract record Expression(TokenPosition Position) : Node(Position);

    /// <summary>
    /// Root node representing the entire program
    /// </summary>
    internal record Program(List<Statement> Statements, TokenPosition Position) : Node(Position);

    /// <summary>
    /// Base class for all value (literal) nodes
    /// </summary>
    internal abstract record Value(TokenPosition Position) : Expression(Position);

    /// <summary>
    /// Node representing an integer number literal
    /// </summary>
    internal record Number(int Value, TokenPosition Position) : Value(Position);

    /// <summary>
    /// Node representing a string literal
    /// </summary>
    internal record String(string Value, TokenPosition Position) : Value(Position);

    /// <summary>
    /// Node representing a boolean literal
    /// </summary>
    internal sealed record Boolean(bool Value, TokenPosition Position) : Value(Position);

    /// <summary>
    /// Node representing a variable reference
    /// </summary>
    internal record Variable(string Name, TokenPosition Position) : Expression(Position);

    /// <summary>
    /// Node representing an identifier (variable name)
    /// </summary>
    internal record Identifier(string Name, TokenPosition Position) : Node(Position);

    // Binary Operations

    /// <summary>
    /// Node representing logical AND operation (a and b)
    /// </summary>
    internal record AndOp(Expression Left, Expression Right, TokenPosition Position) : Expression(Position);

    /// <summary>
    /// Node representing logical OR operation (a or b)
    /// </summary>
    internal record OrOp(Expression Left, Expression Right, TokenPosition Position) : Expression(Position);

    /// <summary>
    /// Node representing logical XOR operation (a xor b)
    /// </summary>
    internal record XorOp(Expression Left, Expression Right, TokenPosition Position) : Expression(Position);

    // Unary Operations

    /// <summary>
    /// Node representing logical NOT operation (not a)
    /// </summary>
    internal record NotOp(Expression Operand, TokenPosition Position) : Expression(Position);

    // Statements

    /// <summary>
    /// Node representing an assignment statement
    /// </summary>
    internal record Assign(Identifier Identifier, Expression Expression, TokenPosition Position) : Statement(Position);

    /// <summary>
    /// Node representing an output statement (<< expression)
    /// </summary>
    internal sealed record Output(Expression Expression, TokenPosition Position) : Statement(Position);
}
