using System.Collections.Generic;

namespace BitPatch.DialogLang.Ast
{
    /// <summary>
    /// Base class for all AST nodes
    /// </summary>
    internal abstract record Node(Location Location)
    {
    }

    /// <summary>
    /// Base class for all statements (executed, do not return values)
    /// </summary>
    internal abstract record Statement(Location Location) : Node(Location);

    /// <summary>
    /// Base class for all expressions (evaluated, return values)
    /// </summary>
    internal abstract record Expression(Location Location) : Node(Location);

    /// <summary>
    /// Root node representing the entire program
    /// </summary>
    internal record Program(List<Statement> Statements, Location Location) : Node(Location);

    /// <summary>
    /// Base class for all value (literal) nodes
    /// </summary>
    internal abstract record Value(Location Location) : Expression(Location);

    /// <summary>
    /// Base class for numeric literals
    /// </summary>
    internal abstract record Number(Location Location) : Value(Location);

    /// <summary>
    /// Node representing an integer literal
    /// </summary>
    internal record Integer(int Value, Location Location) : Number(Location);

    /// <summary>
    /// Node representing a floating-point literal
    /// </summary>
    internal record Float(float Value, Location Location) : Number(Location);

    /// <summary>
    /// Node representing a string literal
    /// </summary>
    internal record String(string Value, Location Location) : Value(Location);

    /// <summary>
    /// Node representing a boolean literal
    /// </summary>
    internal sealed record Boolean(bool Value, Location Location) : Value(Location);

    /// <summary>
    /// Node representing a variable reference
    /// </summary>
    internal record Variable(string Name, Location Location) : Expression(Location);

    /// <summary>
    /// Node representing an identifier (variable name)
    /// </summary>
    internal record Identifier(string Name, Location Location) : Node(Location);

    // Binary Operations

    /// <summary>
    /// Node representing logical AND operation (a and b)
    /// </summary>
    internal record AndOp(Expression Left, Expression Right, Location Location) : Expression(Location);

    /// <summary>
    /// Node representing logical OR operation (a or b)
    /// </summary>
    internal record OrOp(Expression Left, Expression Right, Location Location) : Expression(Location);

    /// <summary>
    /// Node representing logical XOR operation (a xor b)
    /// </summary>
    internal record XorOp(Expression Left, Expression Right, Location Location) : Expression(Location);

    // Comparison Operations

    /// <summary>
    /// Node representing greater than comparison (a > b)
    /// </summary>
    internal record GreaterThanOp(Expression Left, Expression Right, Location Location) : Expression(Location);

    /// <summary>
    /// Node representing less than comparison (a < b)
    /// </summary>
    internal record LessThanOp(Expression Left, Expression Right, Location Location) : Expression(Location);

    /// <summary>
    /// Node representing greater than or equal comparison (a >= b)
    /// </summary>
    internal record GreaterOrEqualOp(Expression Left, Expression Right, Location Location) : Expression(Location);

    /// <summary>
    /// Node representing less than or equal comparison (a <= b)
    /// </summary>
    internal record LessOrEqualOp(Expression Left, Expression Right, Location Location) : Expression(Location);

    /// <summary>
    /// Node representing equality comparison (a == b)
    /// </summary>
    internal record EqualOp(Expression Left, Expression Right, Location Location) : Expression(Location);

    /// <summary>
    /// Node representing inequality comparison (a != b)
    /// </summary>
    internal record NotEqualOp(Expression Left, Expression Right, Location Location) : Expression(Location);

    // Arithmetic Operations

    /// <summary>
    /// Node representing addition operation (a + b)
    /// </summary>
    internal record AddOp(Expression Left, Expression Right, Location Location) : Expression(Location);

    /// <summary>
    /// Node representing subtraction operation (a - b)
    /// </summary>
    internal record SubOp(Expression Left, Expression Right, Location Location) : Expression(Location);

    // Unary Operations

    /// <summary>
    /// Node representing logical NOT operation (not a)
    /// </summary>
    internal record NotOp(Expression Operand, Location Location) : Expression(Location);

    // Statements

    /// <summary>
    /// Node representing an assignment statement
    /// </summary>
    internal record Assign(Identifier Identifier, Expression Expression, Location Location) : Statement(Location);

    /// <summary>
    /// Node representing an output statement (<< expression)
    /// </summary>
    internal sealed record Output(Expression Expression, Location Location) : Statement(Location);
}
