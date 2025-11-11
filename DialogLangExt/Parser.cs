using System;
using System.Collections.Generic;

namespace BitPatch.DialogLang
{
    /// <summary>
    /// Parser that builds an Abstract Syntax Tree from tokens
    /// </summary>
    internal class Parser
    {
        private readonly List<Token> _tokens;
        private int _position;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens ?? throw new ArgumentNullException(nameof(tokens));
            _position = 0;
        }

        /// <summary>
        /// Parses the tokens into an AST
        /// </summary>
        public ProgramNode Parse()
        {
            var program = new ProgramNode();

            while (!IsAtEnd())
            {
                var statement = ParseStatement();
                if (statement != null)
                {
                    program.Statements.Add(statement);
                }
            }

            return program;
        }

        /// <summary>
        /// Parses a single statement
        /// </summary>
        private AstNode? ParseStatement()
        {
            if (IsAtEnd())
            {
                return null;
            }

            // Check if this is an assignment statement
            if (CurrentToken.Type == TokenType.Identifier && PeekNext()?.Type == TokenType.Assign)
            {
                return ParseAssignment();
            }

            throw new Exception($"Unexpected token: {CurrentToken}");
        }

        /// <summary>
        /// Parses an assignment statement: identifier = expression
        /// </summary>
        private AssignNode ParseAssignment()
        {
            var variableName = CurrentToken.Value;
            Advance(); // consume identifier

            if (CurrentToken.Type != TokenType.Assign)
            {
                throw new Exception($"Expected '=' but got {CurrentToken}");
            }
            Advance(); // consume '='

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
            var token = CurrentToken;

            if (token.Type == TokenType.Integer)
            {
                Advance();
                return new NumberNode(int.Parse(token.Value));
            }

            if (token.Type == TokenType.Identifier)
            {
                Advance();
                return new VariableNode(token.Value);
            }

            throw new Exception($"Unexpected token in expression: {token}");
        }

        /// <summary>
        /// Current token being processed
        /// </summary>
        private Token CurrentToken => _tokens[_position];

        /// <summary>
        /// Checks if we've reached the end of tokens
        /// </summary>
        private bool IsAtEnd()
        {
            return _position >= _tokens.Count || CurrentToken.Type == TokenType.EndOfFile;
        }

        /// <summary>
        /// Advances to the next token
        /// </summary>
        private void Advance()
        {
            if (!IsAtEnd())
            {
                _position++;
            }
        }

        /// <summary>
        /// Peeks at the next token without consuming it
        /// </summary>
        private Token? PeekNext()
        {
            if (_position + 1 < _tokens.Count)
            {
                return _tokens[_position + 1];
            }
            return null;
        }
    }
}
