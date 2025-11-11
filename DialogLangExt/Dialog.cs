using System;

namespace BitPatch.DialogLang
{
    /// <summary>
    /// Main entry point for the Game Dialog Script language
    /// </summary>
    public class Dialog
    {
        private readonly Interpreter _interpreter;

        public Dialog()
        {
            _interpreter = new Interpreter();
        }

        /// <summary>
        /// Executes a Game Dialog Script source code
        /// </summary>
        /// <param name="source">The source code to execute</param>
        public void Execute(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            // Tokenize
            var lexer = new Lexer(source);
            var tokens = lexer.Tokenize();

            // Parse
            var parser = new Parser(tokens);
            var ast = parser.Parse();

            // Execute
            _interpreter.Execute(ast);
        }

        /// <summary>
        /// Gets the value of a variable
        /// </summary>
        public object? GetVariable(string name)
        {
            return _interpreter.GetVariable(name);
        }

        /// <summary>
        /// Gets all variables
        /// </summary>
        public System.Collections.Generic.IReadOnlyDictionary<string, object> Variables => _interpreter.Variables;

        /// <summary>
        /// Clears all variables
        /// </summary>
        public void Clear()
        {
            _interpreter.Clear();
        }
    }
}
