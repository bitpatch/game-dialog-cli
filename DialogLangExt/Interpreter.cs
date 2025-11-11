using System;
using System.Collections.Generic;
using BitPatch.DialogLang.Ast;

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
        /// Executes statements one by one as they arrive (streaming)
        /// </summary>
        public void Execute(IEnumerable<Node> statements)
        {
            foreach (var statement in statements)
            {
                ExecuteStatement(statement);
            }
        }

        /// <summary>
        /// Executes a program (legacy method for compatibility)
        /// </summary>
        public void Execute(Program program)
        {
            Execute(program.Statements);
        }

        /// <summary>
        /// Executes a single statement
        /// </summary>
        private void ExecuteStatement(Node node)
        {
            if (node is Assign assign)
            {
                ExecuteAssignment(assign);
            }
            else
            {
                throw new ScriptException($"Unknown statement type: {node.GetType().Name}", 0, 0);
            }
        }

        /// <summary>
        /// Executes an assignment statement
        /// </summary>
        private void ExecuteAssignment(Assign node)
        {
            var value = EvaluateExpression(node.Expression);
            _variables[node.Identifier.Name] = value;
        }

        /// <summary>
        /// Evaluates an expression and returns its value
        /// </summary>
        private object EvaluateExpression(Expression expression)
        {
            if (expression is Number number)
            {
                return number.Value;
            }

            if (expression is Variable variable)
            {
                if (_variables.TryGetValue(variable.Name, out var value))
                {
                    return value;
                }
                throw new ScriptException($"Variable '{variable.Name}' is not defined", expression.Line, expression.Column);
            }

            throw new ScriptException($"Unknown expression type: {expression.GetType().Name}", expression.Line, expression.Column);
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
