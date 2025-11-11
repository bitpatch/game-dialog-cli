using System.Collections.Generic;

namespace BitPatch.DialogLang
{
    /// <summary>
    /// Parser that builds an Abstract Syntax Tree from tokens
    /// </summary>
    internal class Parser
    {
        private readonly IEnumerator<Token> _tokens;
        private Token _current;
        private Token _next;

        public Parser(IEnumerable<Token> tokens)
        {
            _tokens = tokens.GetEnumerator();
            _current = new Token(TokenType.EndOfFile, string.Empty, 0, 0);
            _next = new Token(TokenType.EndOfFile, string.Empty, 0, 0);

            // Initialize first two tokens
            MoveNext();
            MoveNext();
        }

        /// <summary>
        /// Parses tokens and yields statements one by one (streaming)
        /// </summary>
        public IEnumerable<Ast.Node> Parse()
        {
            // Skip leading newlines
            SkipNewlines();

            while (!IsAtEnd())
            {
                yield return ParseStatement();

                // Expect newline or EOF after statement
                if (!IsAtEnd() && _current.Type != TokenType.Newline)
                {
                    throw new ScriptException($"Expected newline or end of file after statement, but got {_current.Type}", _current.Line, _current.Column);
                }

                // Skip newlines after statement
                SkipNewlines();
            }
        }

        /// <summary>
        /// Parses a single statement
        /// </summary>
        private Ast.Node ParseStatement()
        {
            // Check if this is an assignment statement
            if (_current.Type == TokenType.Identifier && _next.Type == TokenType.Assign)
            {
                return ParseAssignment();
            }

            throw new ScriptException($"Unexpected token: {_current.Type}", _current.Line, _current.Column);
        }

        /// <summary>
        /// Parses an assignment statement: identifier = expression
        /// </summary>
        private Ast.Assign ParseAssignment()
        {
            var line = _current.Line;
            var column = _current.Column;

            var identifier = ParseIdentifier();

            if (_current.Type != TokenType.Assign)
            {
                throw new ScriptException($"Expected '=' but got {_current.Type}", _current.Line, _current.Column);
            }
            MoveNext(); // consume '='

            var expression = ParseExpression();
            return new Ast.Assign(identifier, expression, line, column);
        }

        private Ast.Identifier ParseIdentifier()
        {
            var token = _current;

            if (token.Type != TokenType.Identifier)
            {
                throw new ScriptException($"Expected identifier but got {token.Type}", token.Line, token.Column);
            }

            MoveNext(); // consume identifier
            return new Ast.Identifier(token.Value, token.Line, token.Column);
        }

        /// <summary>
        /// Parses an expression (for now, just literals)
        /// </summary>
        private Ast.Expression ParseExpression()
        {
            var token = _current;

            if (token.Type == TokenType.Integer)
            {
                MoveNext();
                return new Ast.Number(int.Parse(token.Value), token.Line, token.Column);
            }

            if (token.Type == TokenType.Identifier)
            {
                MoveNext();
                return new Ast.Variable(token.Value, token.Line, token.Column);
            }

            throw new ScriptException($"Unexpected token in expression: {token.Type}", token.Line, token.Column);
        }

        /// <summary>
        /// Checks if we've reached the end of tokens
        /// </summary>
        private bool IsAtEnd()
        {
            return _current.Type == TokenType.EndOfFile;
        }

        /// <summary>
        /// Moves to the next token
        /// </summary>
        private void MoveNext()
        {
            _current = _next;

            if (_tokens.MoveNext())
            {
                _next = _tokens.Current;
            }
            else
            {
                _next = new Token(TokenType.EndOfFile, string.Empty, _current.Line, _current.Column);
            }
        }

        /// <summary>
        /// Skips all consecutive newline tokens
        /// </summary>
        private void SkipNewlines()
        {
            while (_current.Type == TokenType.Newline)
            {
                MoveNext();
            }
        }
    }
}
