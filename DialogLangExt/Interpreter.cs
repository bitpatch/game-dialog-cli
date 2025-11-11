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
                throw new Exception($"Unknown statement type: {node.GetType().Name}");
            }
        }

        /// <summary>
        /// Executes an assignment statement
        /// </summary>
        private void ExecuteAssignment(Assign node)
        {
            var value = EvaluateExpression(node.Value);
            _variables[node.VariableName] = value;
        }

        /// <summary>
        /// Evaluates an expression and returns its value
        /// </summary>
        private object EvaluateExpression(Node node)
        {
            if (node is Number number)
            {
                return number.Value;
            }

            if (node is Variable variable)
            {
                if (_variables.TryGetValue(variable.Name, out var value))
                {
                    return value;
                }
                throw new Exception($"Variable '{variable.Name}' is not defined");
            }

            throw new Exception($"Unknown expression type: {node.GetType().Name}");
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
