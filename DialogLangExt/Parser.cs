using System;
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
            _current = new Token(TokenType.EndOfFile, string.Empty, 0);
            _next = new Token(TokenType.EndOfFile, string.Empty, 0);
            
            // Initialize first two tokens
            MoveNext();
            MoveNext();
        }

        /// <summary>
        /// Parses tokens and yields statements one by one (streaming)
        /// </summary>
        public IEnumerable<AstNode> Parse()
        {
            while (!IsAtEnd())
            {
                yield return ParseStatement();
            }
        }

        /// <summary>
        /// Parses a single statement
        /// </summary>
        private AstNode ParseStatement()
        {
            // Check if this is an assignment statement
            if (_current.Type == TokenType.Identifier && _next.Type == TokenType.Assign)
            {
                return ParseAssignment();
            }

            throw new Exception($"Unexpected token: {_current}");
        }

        /// <summary>
        /// Parses an assignment statement: identifier = expression
        /// </summary>
        private AssignNode ParseAssignment()
        {
            var variableName = _current.Value;
            MoveNext(); // consume identifier

            if (_current.Type != TokenType.Assign)
            {
                throw new Exception($"Expected '=' but got {_current}");
            }
            MoveNext(); // consume '='

            var value = ParseExpression();

            return new AssignNode(variableName, value);
        }

        /// <summary>
        /// Parses an expression (for now, just literals)
        /// </summary>
        private AstNode ParseExpression()
        {
            return ParsePrimary();
        }

        /// <summary>
        /// Parses primary expressions (numbers, variables)
        /// </summary>
        private AstNode ParsePrimary()
        {
            var token = _current;

            if (token.Type == TokenType.Integer)
            {
                MoveNext();
                return new NumberNode(int.Parse(token.Value));
            }

            if (token.Type == TokenType.Identifier)
            {
                MoveNext();
                return new VariableNode(token.Value);
            }

            throw new Exception($"Unexpected token in expression: {token}");
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
                _next = new Token(TokenType.EndOfFile, string.Empty, _current.Position);
            }
        }
    }
}
