using System.Collections.Generic;

namespace BitPatch.DialogLang.Ast
{
    /// <summary>
    /// Base class for all AST nodes.
    /// </summary>
    internal abstract record Node(Location Location)
    {
    }

    /// <summary>
    /// Indicates that the expression may return a boolean value.
    /// </summary>
    internal interface IBoolean { }

    /// <summary>
    /// Base class for all statements (executed, do not return values).
    /// </summary>
    internal abstract record Statement(Location Location) : Node(Location);

    /// <summary>
    /// Base class for all expressions (evaluated, return values).
    /// </summary>
    internal abstract record Expression(Location Location) : Node(Location);

    /// <summary>
    /// Root node representing the entire program.
    /// </summary>
    internal sealed record Program(List<Statement> Statements, Location Location) : Node(Location);

    /// <summary>
    /// Base class for numeric literals.
    /// </summary>
    internal abstract record Number(Location Location) : Expression(Location);

    /// <summary>
    /// Node representing an integer literal.
    /// </summary>
    internal sealed record Integer(int Value, Location Location) : Number(Location);

    /// <summary>
    /// Node representing a floating-point literal.
    /// </summary>
    internal sealed record Float(float Value, Location Location) : Number(Location);

    /// <summary>
    /// Node representing a string literal.
    /// </summary>
    internal sealed record String(string Value, Location Location) : Expression(Location);

    /// <summary>
    /// Node representing a boolean literal.
    /// </summary>
    internal sealed record Boolean(bool Value, Location Location) : Expression(Location), IBoolean;

    /// <summary>
    /// Node representing a variable reference.
    /// </summary>
    internal sealed record Variable(string Name, Location Location) : Expression(Location), IBoolean;

    /// <summary>
    /// Node representing an identifier (variable name).
    /// </summary>
    internal sealed record Identifier(string Name, Location Location) : Node(Location);

    // Binary Operations

    /// <summary>
    /// Node representing logical AND operation (a and b).
    /// </summary>
    internal sealed record AndOp(Expression Left, Expression Right, Location Location) : Expression(Location), IBoolean;

    /// <summary>
    /// Node representing logical OR operation (a or b).
    /// </summary>
    internal sealed record OrOp(Expression Left, Expression Right, Location Location) : Expression(Location), IBoolean;

    /// <summary>
    /// Node representing logical XOR operation (a xor b).
    /// </summary>
    internal sealed record XorOp(Expression Left, Expression Right, Location Location) : Expression(Location), IBoolean;

    // Comparison Operations

    /// <summary>
    /// Node representing greater than comparison (a > b).
    /// </summary>
    internal sealed record GreaterThanOp(Expression Left, Expression Right, Location Location) : Expression(Location), IBoolean;

    /// <summary>
    /// Node representing less than comparison (a < b).
    /// </summary>
    internal sealed record LessThanOp(Expression Left, Expression Right, Location Location) : Expression(Location), IBoolean;

    /// <summary>
    /// Node representing greater than or equal comparison (a >= b).
    /// </summary>
    internal sealed record GreaterOrEqualOp(Expression Left, Expression Right, Location Location) : Expression(Location), IBoolean;

    /// <summary>
    /// Node representing less than or equal comparison (a <= b).
    /// </summary>
    internal sealed record LessOrEqualOp(Expression Left, Expression Right, Location Location) : Expression(Location), IBoolean;

    /// <summary>
    /// Node representing equality comparison (a == b).
    /// </summary>
    internal sealed record EqualOp(Expression Left, Expression Right, Location Location) : Expression(Location), IBoolean;

    /// <summary>
    /// Node representing inequality comparison (a != b).
    /// </summary>
    internal sealed record NotEqualOp(Expression Left, Expression Right, Location Location) : Expression(Location), IBoolean;

    // Arithmetic Operations

    /// <summary>
    /// Node representing addition operation (a + b).
    /// </summary>
    internal sealed record AddOp(Expression Left, Expression Right, Location Location) : Expression(Location);

    /// <summary>
    /// Node representing subtraction operation (a - b).
    /// </summary>
    internal sealed record SubOp(Expression Left, Expression Right, Location Location) : Expression(Location);

    /// <summary>
    /// Node representing multiplication operation (a * b).
    /// </summary>
    internal sealed record MulOp(Expression Left, Expression Right, Location Location) : Expression(Location);

    /// <summary>
    /// Node representing division operation (a / b).
    /// </summary>
    internal sealed record DivOp(Expression Left, Expression Right, Location Location) : Expression(Location);

    /// <summary>
    /// Node representing modulo operation (a % b).
    /// </summary>
    internal sealed record ModOp(Expression Left, Expression Right, Location Location) : Expression(Location);

    // Unary Operations

    /// <summary>
    /// Node representing logical NOT operation (not a).
    /// </summary>
    internal sealed record NotOp(Expression Operand, Location Location) : Expression(Location);

    /// <summary>
    /// Node representing unary negation operation (-a).
    /// </summary>
    internal sealed record NegateOp(Expression Operand, Location Location) : Expression(Location);

    // Statements

    /// <summary>
    /// Node representing an assignment statement.
    /// </summary>
    internal sealed record Assign(Identifier Identifier, Expression Expression, Location Location) : Statement(Location);

    /// <summary>
    /// Node representing an output statement (<< expression).
    /// </summary>
    internal sealed record Output(Expression Expression, Location Location) : Statement(Location);

    /// <summary>
    /// Node representing a block of statements grouped by indentation.
    /// </summary>
    internal sealed record Block(IReadOnlyList<Statement> Statements, Location Location) : Statement(Location);

    /// <summary>
    /// Node representing a while loop statement.
    /// </summary>
    internal sealed record While(Expression Condition, Block Body, Location Location) : Statement(Location);
}
