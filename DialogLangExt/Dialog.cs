using System;
using System.Collections.Generic;
using System.IO;

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
        /// Executes a Game Dialog Script source code from a TextReader (streaming mode)
        /// Yields output values from << statements
        /// </summary>
        /// <param name="reader">The TextReader to read source code from</param>
        /// <returns>Enumerable of output values</returns>
        public IEnumerable<object> Execute(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            // Tokenize (streaming)
            var lexer = new Lexer(reader);
            var tokens = lexer.Tokenize();

            // Parse (streaming)
            var parser = new Parser(tokens);
            var statements = parser.Parse();

            // Execute (streaming) - yields output values from << statements
            return _interpreter.Execute(statements);
        }

        /// <summary>
        /// Executes a Game Dialog Script source code from a string
        /// Yields output values from << statements
        /// </summary>
        /// <param name="source">The source code to execute</param>
        /// <returns>Enumerable of output values</returns>
        public IEnumerable<object> Execute(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            using var reader = new StringReader(source);
            return Execute(reader);
        }

        /// <summary>
        /// Gets the value of a variable
        /// </summary>
        public object? GetVariable(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

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
