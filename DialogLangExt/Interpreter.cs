using System;
using System.Collections.Generic;

namespace BitPatch.DialogLang
{
    /// <summary>
    /// Interpreter that executes the AST
    /// </summary>
    internal class Interpreter
    {
        private readonly Dictionary<string, object> _variables;

        public Interpreter()
        {
            _variables = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets all variables in the current scope
        /// </summary>
        public IReadOnlyDictionary<string, object> Variables => _variables;

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
                        yield return EvaluateExpression(output.Expression);
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
        private object EvaluateExpression(Ast.Expression expression)
        {
            return expression switch
            {
                Ast.Number number => number.Value,
                Ast.String str => str.Value,
                Ast.Boolean boolean => boolean.Value,
                Ast.Variable variable => EvaluateVariable(variable),
                Ast.AndOp andOp => EvaluateAndOp(andOp),
                Ast.OrOp orOp => EvaluateOrOp(orOp),
                Ast.XorOp xorOp => EvaluateXorOp(xorOp),
                Ast.NotOp notOp => EvaluateNotOp(notOp),
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

            if (left is not bool leftBool)
            {
                throw new ScriptException($"Left operand of 'and' must be boolean, got {left.GetType().Name}", andOp.Position);
            }

            if (right is not bool rightBool)
            {
                throw new ScriptException($"Right operand of 'and' must be boolean, got {right.GetType().Name}", andOp.Position);
            }

            return leftBool && rightBool;
        }

        /// <summary>
        /// Evaluates logical OR operation
        /// </summary>
        private bool EvaluateOrOp(Ast.OrOp orOp)
        {
            var left = EvaluateExpression(orOp.Left);
            var right = EvaluateExpression(orOp.Right);

            if (left is not bool leftBool)
            {
                throw new ScriptException($"Left operand of 'or' must be boolean, got {left.GetType().Name}", orOp.Position);
            }

            if (right is not bool rightBool)
            {
                throw new ScriptException($"Right operand of 'or' must be boolean, got {right.GetType().Name}", orOp.Position);
            }

            return leftBool || rightBool;
        }

        /// <summary>
        /// Evaluates logical XOR operation
        /// </summary>
        private bool EvaluateXorOp(Ast.XorOp xorOp)
        {
            var left = EvaluateExpression(xorOp.Left);
            var right = EvaluateExpression(xorOp.Right);

            if (left is not bool leftBool)
            {
                throw new ScriptException($"Left operand of 'xor' must be boolean, got {left.GetType().Name}", xorOp.Position);
            }

            if (right is not bool rightBool)
            {
                throw new ScriptException($"Right operand of 'xor' must be boolean, got {right.GetType().Name}", xorOp.Position);
            }

            return leftBool ^ rightBool;
        }

        /// <summary>
        /// Evaluates logical NOT operation
        /// </summary>
        private bool EvaluateNotOp(Ast.NotOp notOp)
        {
            var operand = EvaluateExpression(notOp.Operand);

            if (operand is not bool boolOperand)
            {
                throw new ScriptException($"Operand of 'not' must be boolean, got {operand.GetType().Name}", notOp.Position);
            }

            return !boolOperand;
        }

        private object EvaluateVariable(Ast.Variable variable)
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
            return _variables.TryGetValue(name, out var value) ? value : null;
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
