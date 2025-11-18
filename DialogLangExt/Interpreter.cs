using System;
using System.Collections.Generic;

namespace BitPatch.DialogLang
{
    /// <summary>
    /// Interpreter that executes the AST
    /// </summary>
    internal class Interpreter
    {
        private readonly Dictionary<string, RuntimeValue> _variables;

        public Interpreter()
        {
            _variables = new Dictionary<string, RuntimeValue>();
        }

        /// <summary>
        /// Gets all variables in the current scope
        /// </summary>
        public IReadOnlyDictionary<string, RuntimeValue> Variables => _variables;

        /// <summary>
        /// Executes statements one by one as they arrive (streaming), yielding output values
        /// </summary>
        public IEnumerable<object> Execute(IEnumerable<Ast.Statement> statements)
        {
            foreach (var statement in statements)
            {
                switch (statement)
                {
                    case Ast.Output output:
                        yield return EvaluateExpression(output.Expression).ToObject();
                        break;
                    case Ast.Assign assign:
                        ExecuteAssignment(assign);
                        break;
                    default:
                        throw new NotSupportedException($"Unsupported statement type: {statement.GetType().Name}");
                }
            }
        }

        /// <summary>
        /// Executes a program (legacy method for compatibility)
        /// </summary>
        public IEnumerable<object> Execute(Ast.Program program)
        {
            return Execute(program.Statements);
        }

        /// <summary>
        /// Executes an assignment statement
        /// </summary>
        private void ExecuteAssignment(Ast.Assign node)
        {
            _variables[node.Identifier.Name] = EvaluateExpression(node.Expression);
        }

        /// <summary>
        /// Evaluates an expression and returns its value
        /// </summary>
        private RuntimeValue EvaluateExpression(Ast.Expression expression)
        {
            return expression switch
            {
                Ast.Integer integer => new Integer(integer.Value),
                Ast.Float floatNode => new Float(floatNode.Value),
                Ast.String str => new String(str.Value),
                Ast.Boolean boolean => new Boolean(boolean.Value),
                Ast.Variable variable => EvaluateVariable(variable),
                Ast.AndOp andOp => new Boolean(EvaluateAndOp(andOp)),
                Ast.OrOp orOp => new Boolean(EvaluateOrOp(orOp)),
                Ast.XorOp xorOp => new Boolean(EvaluateXorOp(xorOp)),
                Ast.NotOp notOp => new Boolean(EvaluateNotOp(notOp)),
                Ast.GreaterThanOp greaterThan => EvaluateGreaterThanOp(greaterThan),
                Ast.LessThanOp lessThan => EvaluateLessThanOp(lessThan),
                Ast.GreaterOrEqualOp greaterOrEqual => EvaluateGreaterOrEqualOp(greaterOrEqual),
                Ast.LessOrEqualOp lessOrEqual => EvaluateLessOrEqualOp(lessOrEqual),
                Ast.EqualOp equal => EvaluateEqualOp(equal),
                Ast.NotEqualOp notEqual => EvaluateNotEqualOp(notEqual),
                Ast.AddOp addOp => EvaluateAddOp(addOp),
                Ast.SubOp subOp => EvaluateSubOp(subOp),
                _ => throw new NotSupportedException($"Unsupported expression type: {expression.GetType().Name}")
            };
        }

        /// <summary>
        /// Evaluates logical AND operation
        /// </summary>
        private bool EvaluateAndOp(Ast.AndOp andOp)
        {
            var left = EvaluateExpression(andOp.Left);
            var right = EvaluateExpression(andOp.Right);

            if (left is not Boolean leftBool)
            {
                throw new TypeMismatchException(typeof(Boolean), left, andOp.Left.Position);
            }

            if (right is not Boolean rightBool)
            {
                throw new TypeMismatchException(typeof(Boolean), right, andOp.Right.Position);
            }

            return leftBool.Value && rightBool.Value;
        }

        /// <summary>
        /// Evaluates logical OR operation
        /// </summary>
        private bool EvaluateOrOp(Ast.OrOp orOp)
        {
            var left = EvaluateExpression(orOp.Left);
            var right = EvaluateExpression(orOp.Right);

            if (left is not Boolean leftBool)
            {
                throw new TypeMismatchException(typeof(Boolean), left, orOp.Left.Position);
            }

            if (right is not Boolean rightBool)
            {
                throw new TypeMismatchException(typeof(Boolean), right, orOp.Right.Position);
            }

            return leftBool.Value || rightBool.Value;
        }

        /// <summary>
        /// Evaluates logical XOR operation
        /// </summary>
        private bool EvaluateXorOp(Ast.XorOp xorOp)
        {
            var left = EvaluateExpression(xorOp.Left);
            var right = EvaluateExpression(xorOp.Right);

            if (left is not Boolean leftBool)
            {
                throw new TypeMismatchException(typeof(Boolean), left, xorOp.Left.Position);
            }

            if (right is not Boolean rightBool)
            {
                throw new TypeMismatchException(typeof(Boolean), right, xorOp.Right.Position);
            }

            return leftBool.Value ^ rightBool.Value;
        }

        /// <summary>
        /// Evaluates logical NOT operation
        /// </summary>
        private bool EvaluateNotOp(Ast.NotOp notOp)
        {
            var operand = EvaluateExpression(notOp.Operand);

            if (operand is not Boolean boolOperand)
            {
                throw new TypeMismatchException(typeof(Boolean), operand, notOp.Operand.Position);
            }

            return !boolOperand.Value;
        }

        /// <summary>
        /// Evaluates greater than comparison (>)
        /// </summary>
        private Boolean EvaluateGreaterThanOp(Ast.GreaterThanOp op)
        {
            var left = EvaluateExpression(op.Left);
            var right = EvaluateExpression(op.Right);
            var diff = CompareNumeric(left, right, op.Left.Position, op.Right.Position);
            return new Boolean(diff > float.Epsilon);
        }

        /// <summary>
        /// Evaluates less than comparison (<)
        /// </summary>
        private Boolean EvaluateLessThanOp(Ast.LessThanOp op)
        {
            var left = EvaluateExpression(op.Left);
            var right = EvaluateExpression(op.Right);
            var diff = CompareNumeric(left, right, op.Left.Position, op.Right.Position);
            return new Boolean(diff < -float.Epsilon);
        }

        /// <summary>
        /// Evaluates greater than or equal comparison (>=)
        /// </summary>
        private Boolean EvaluateGreaterOrEqualOp(Ast.GreaterOrEqualOp op)
        {
            var left = EvaluateExpression(op.Left);
            var right = EvaluateExpression(op.Right);
            var diff = CompareNumeric(left, right, op.Left.Position, op.Right.Position);
            return new Boolean(diff > -float.Epsilon);
        }

        /// <summary>
        /// Evaluates less than or equal comparison (<=)
        /// </summary>
        private Boolean EvaluateLessOrEqualOp(Ast.LessOrEqualOp op)
        {
            var left = EvaluateExpression(op.Left);
            var right = EvaluateExpression(op.Right);
            var diff = CompareNumeric(left, right, op.Left.Position, op.Right.Position);
            return new Boolean(diff < float.Epsilon);
        }

        /// <summary>
        /// Evaluates equality comparison (==)
        /// </summary>
        private Boolean EvaluateEqualOp(Ast.EqualOp op)
        {
            var left = EvaluateExpression(op.Left);
            var right = EvaluateExpression(op.Right);
            return new Boolean(AreEqual(left, right));
        }

        /// <summary>
        /// Evaluates inequality comparison (!=)
        /// </summary>
        private Boolean EvaluateNotEqualOp(Ast.NotEqualOp op)
        {
            var left = EvaluateExpression(op.Left);
            var right = EvaluateExpression(op.Right);
            return new Boolean(!AreEqual(left, right));
        }

        /// <summary>
        /// Evaluates addition operation (+)
        /// </summary>
        private RuntimeValue EvaluateAddOp(Ast.AddOp op)
        {
            var left = EvaluateExpression(op.Left);
            var right = EvaluateExpression(op.Right);

            // String concatenation with type conversion
            if (left is String || right is String)
            {
                return new String(ConvertToString(left) + ConvertToString(right));
            }

            // Numeric addition
            return (left, right) switch
            {
                (Integer l, Integer r) => new Integer(l.Value + r.Value),
                (Float l, Float r) => new Float(l.Value + r.Value),
                (Integer l, Float r) => new Float(l.Value + r.Value),
                (Float l, Integer r) => new Float(l.Value + r.Value),
                (Number, _) => throw new TypeMismatchException(typeof(Number), right, op.Right.Position),
                _ => throw new TypeMismatchException(typeof(Number), left, op.Left.Position)
            };
        }

        /// <summary>
        /// Evaluates subtraction operation (-)
        /// </summary>
        private RuntimeValue EvaluateSubOp(Ast.SubOp op)
        {
            var left = EvaluateExpression(op.Left);
            var right = EvaluateExpression(op.Right);

            return (left, right) switch
            {
                (Integer l, Integer r) => new Integer(l.Value - r.Value),
                (Float l, Float r) => new Float(l.Value - r.Value),
                (Integer l, Float r) => new Float(l.Value - r.Value),
                (Float l, Integer r) => new Float(l.Value - r.Value),
                (Number, _) => throw new TypeMismatchException(typeof(Number), right, op.Right.Position),
                _ => throw new TypeMismatchException(typeof(Number), left, op.Left.Position)
            };
        }

        /// <summary>
        /// Converts a runtime value to string representation
        /// </summary>
        private string ConvertToString(RuntimeValue value)
        {
            return value switch
            {
                String s => s.Value,
                Integer i => i.Value.ToString(System.Globalization.CultureInfo.InvariantCulture),
                Float f => f.Value.ToString(System.Globalization.CultureInfo.InvariantCulture),
                Boolean b => b.Value ? "true" : "false",
                _ => throw new NotSupportedException($"Cannot convert {value.GetType().Name} to string")
            };
        }

        /// <summary>
        /// Compares two numeric values
        /// </summary>
        private float CompareNumeric(RuntimeValue left, RuntimeValue right, TokenPosition leftPos, TokenPosition rightPos)
        {
            return (left, right) switch
            {
                (Integer l, Integer r) => l.Value - r.Value,
                (Float l, Float r) => l.Value - r.Value,
                (Integer l, Float r) => l.Value - r.Value,
                (Float l, Integer r) => l.Value - r.Value,
                _ => throw new TypeMismatchException(typeof(Number), left, leftPos)
            };
        }

        /// <summary>
        /// Checks if two runtime values are equal
        /// </summary>
        private bool AreEqual(RuntimeValue left, RuntimeValue right)
        {
            return (left, right) switch
            {
                (Integer l, Integer r) => l.Value == r.Value,
                (Float l, Float r) => Math.Abs(l.Value - r.Value) < float.Epsilon,
                (Integer l, Float r) => Math.Abs(l.Value - r.Value) < float.Epsilon,
                (Float l, Integer r) => Math.Abs(l.Value - r.Value) < float.Epsilon,
                (String l, String r) => l.Value == r.Value,
                (Boolean l, Boolean r) => l.Value == r.Value,
                _ => false
            };
        }

        private RuntimeValue EvaluateVariable(Ast.Variable variable)
        {
            if (_variables.TryGetValue(variable.Name, out var value))
            {
                return value;
            }

            throw new ScriptException($"Variable '{variable.Name}' is not defined", variable.Position);
        }

        /// <summary>
        /// Gets the value of a variable
        /// </summary>
        public object? GetVariable(string name)
        {
            return _variables.TryGetValue(name, out var value) ? value.ToObject() : null;
        }

        /// <summary>
        /// Clears all variables
        /// </summary>
        public void Clear()
        {
            _variables.Clear();
        }
    }
}
