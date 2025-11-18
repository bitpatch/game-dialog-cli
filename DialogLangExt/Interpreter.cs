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
                Ast.Number number => new Number(number.Value),
                Ast.String str => new String(str.Value),
                Ast.Boolean boolean => new Boolean(boolean.Value),
                Ast.Variable variable => EvaluateVariable(variable),
                Ast.AndOp andOp => new Boolean(EvaluateAndOp(andOp)),
                Ast.OrOp orOp => new Boolean(EvaluateOrOp(orOp)),
                Ast.XorOp xorOp => new Boolean(EvaluateXorOp(xorOp)),
                Ast.NotOp notOp => new Boolean(EvaluateNotOp(notOp)),
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
