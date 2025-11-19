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
                Ast.MulOp mulOp => EvaluateMulOp(mulOp),
                Ast.DivOp divOp => EvaluateDivOp(divOp),
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

            return (left, right) switch
            {
                (Boolean l, Boolean r) => l.Value && r.Value,
                (Boolean l, not Boolean) => throw Exception(andOp.Right.Location),
                (not Boolean, Boolean r) => throw Exception(andOp.Left.Location),
                _ => throw Exception(andOp.Left.Location | andOp.Right.Location)
            };

            ScriptException Exception(Location location)
            {
                return new ScriptException($"Cannot evaluate AND operation with types {left.GetType().Name} and {right.GetType().Name}", location);
            }
        }

        /// <summary>
        /// Evaluates logical OR operation
        /// </summary>
        private bool EvaluateOrOp(Ast.OrOp orOp)
        {
            var left = EvaluateExpression(orOp.Left);
            var right = EvaluateExpression(orOp.Right);

            return (left, right) switch
            {
                (Boolean l, Boolean r) => l.Value || r.Value,
                (Boolean l, not Boolean) => throw Exception(orOp.Right.Location),
                (not Boolean, Boolean r) => throw Exception(orOp.Left.Location),
                _ => throw Exception(orOp.Left.Location | orOp.Right.Location)
            };

            ScriptException Exception(Location location)
            {
                return new ScriptException($"Cannot evaluate OR operation with types {left.GetType().Name} and {right.GetType().Name}", location);
            }
        }

        /// <summary>
        /// Evaluates logical XOR operation
        /// </summary>
        private bool EvaluateXorOp(Ast.XorOp xorOp)
        {
            var left = EvaluateExpression(xorOp.Left);
            var right = EvaluateExpression(xorOp.Right);

            return (left, right) switch
            {
                (Boolean l, Boolean r) => l.Value ^ r.Value,
                (Boolean l, not Boolean) => throw Exception(xorOp.Right.Location),
                (not Boolean, Boolean r) => throw Exception(xorOp.Left.Location),
                _ => throw Exception(xorOp.Left.Location | xorOp.Right.Location)
            };

            ScriptException Exception(Location location)
            {
                return new ScriptException($"Cannot evaluate XOR operation with types {left.GetType().Name} and {right.GetType().Name}", location);
            }
        }

        /// <summary>
        /// Evaluates logical NOT operation
        /// </summary>
        private bool EvaluateNotOp(Ast.NotOp notOp)
        {
            var operand = EvaluateExpression(notOp.Operand);

            if (operand is not Boolean boolOperand)
            {
                throw new ScriptException($"Cannot evaluate NOT operation on type {operand.GetType().Name}", notOp.Operand.Location);
            }

            return !boolOperand.Value;
        }

        /// <summary>
        /// Evaluates greater than comparison (>)
        /// </summary>
        private Boolean EvaluateGreaterThanOp(Ast.GreaterThanOp op)
        {
            var diff = CompareNumeric(op.Left, op.Right);
            return new Boolean(diff > float.Epsilon);
        }

        /// <summary>
        /// Evaluates less than comparison (<)
        /// </summary>
        private Boolean EvaluateLessThanOp(Ast.LessThanOp op)
        {
            var diff = CompareNumeric(op.Left, op.Right);
            return new Boolean(diff < -float.Epsilon);
        }

        /// <summary>
        /// Evaluates greater than or equal comparison (>=)
        /// </summary>
        private Boolean EvaluateGreaterOrEqualOp(Ast.GreaterOrEqualOp op)
        {
            var diff = CompareNumeric(op.Left, op.Right);
            return new Boolean(diff > -float.Epsilon);
        }

        /// <summary>
        /// Evaluates less than or equal comparison (<=)
        /// </summary>
        private Boolean EvaluateLessOrEqualOp(Ast.LessOrEqualOp op)
        {
            var diff = CompareNumeric(op.Left, op.Right);
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
                return new String(left.ToString() + right.ToString());
            }

            // Numeric addition
            return (left, right) switch
            {
                (Integer l, Integer r) => new Integer(l.Value + r.Value),
                (Number l, Number r) => new Float(l.FloatValue + r.FloatValue),
                (Number, not Number) => throw Exception(op.Right.Location),
                (not Number, Number) => throw Exception(op.Left.Location),
                _ => throw Exception(op.Left.Location | op.Right.Location)
            };

            ScriptException Exception(Location location)
            {
                return new ScriptException($"Cannot add {left.GetType().Name} and {right.GetType().Name}", location);
            }
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
                (Number l, Number r) => new Float(l.FloatValue - r.FloatValue),
                (Number, not Number) => throw Exception(op.Right.Location),
                (not Number, Number) => throw Exception(op.Left.Location),
                _ => throw Exception(op.Left.Location | op.Right.Location)
            };

            ScriptException Exception(Location location)
            {
                return new ScriptException($"Cannot subtract {right.GetType().Name} from {left.GetType().Name}", location);
            }
        }

        /// <summary>
        /// Evaluates multiplication operation (*)
        /// </summary>
        private RuntimeValue EvaluateMulOp(Ast.MulOp op)
        {
            var left = EvaluateExpression(op.Left);
            var right = EvaluateExpression(op.Right);

            return (left, right) switch
            {
                (Integer l, Integer r) => new Integer(l.Value * r.Value),
                (Number l, Number r) => new Float(l.FloatValue * r.FloatValue),
                (Number, not Number) => throw Exception(op.Right.Location),
                (not Number, Number) => throw Exception(op.Left.Location),
                _ => throw Exception(op.Left.Location | op.Right.Location)
            };

            ScriptException Exception(Location location)
            {
                return new ScriptException($"Cannot multiply {left.GetType().Name} by {right.GetType().Name}", location);
            }
        }

        /// <summary>
        /// Evaluates division operation (/)
        /// </summary>
        private RuntimeValue EvaluateDivOp(Ast.DivOp op)
        {
            var left = EvaluateExpression(op.Left);
            var right = EvaluateExpression(op.Right);

            // Division always returns float to handle cases like 5 / 2 = 2.5
            return (left, right) switch
            {
                (_, Number n) when n.IsNil => throw new ScriptException("Division by zero", op.Right.Location),
                (Number l, Number r) when !r.IsNil => new Float(l.FloatValue / r.FloatValue),
                (Number, not Number) => throw Exception(op.Right.Location),
                (not Number, Number) => throw Exception(op.Left.Location),
                _ => throw Exception(op.Left.Location | op.Right.Location)
            };

            ScriptException Exception(Location location)
            {
                return new ScriptException($"Cannot divide {left.GetType().Name} by {right.GetType().Name}", location);
            }
        }

        /// <summary>
        /// Compares two numeric values
        /// </summary>
        private float CompareNumeric(Ast.Expression left, Ast.Expression right)
        {
            var leftValue = EvaluateExpression(left);
            var rightValue = EvaluateExpression(right);

            return (leftValue, rightValue) switch
            {
                (Integer l, Integer r) => l.Value - r.Value,
                (Number l, Number r) => l.FloatValue - r.FloatValue,
                (not Number, Number) => throw Exception(left.Location),
                (Number, not Number) => throw Exception(right.Location),
                _ => throw Exception(left.Location | right.Location)

            };

            ScriptException Exception(Location location)
            {
                return new ScriptException($"Cannot compare {left.GetType().Name} and {right.GetType().Name}", location);
            }
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

            throw new ScriptException($"Variable '{variable.Name}' is not defined", variable.Location);
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
