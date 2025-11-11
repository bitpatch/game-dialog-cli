using System.Collections.Generic;
using System;

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

            while (!_current.IsEndOfFile())
            {
                yield return ParseStatement();

                // Expect newline or EOF after statement
                if (!_current.IsEndOfStatement())
                {
                    throw new InvalidSyntaxException(_current.Position);
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
            return _current.Type switch
            {
                TokenType.Identifier => ParseStatementFromIdentifier(),
                TokenType.Output => ParseOutput(),
                _ => throw new InvalidSyntaxException(_current.Position)
            };
        }

        private Ast.Node ParseStatementFromIdentifier()
        {
            if (_current.Type != TokenType.Identifier)
            {
                throw new InvalidOperationException($"Expected identifier token, but current token type is {_current.Type}");
            }

            return _next.Type switch
            {
                TokenType.Assign => ParseAssignment(),
                _ => throw new InvalidSyntaxException(_current.Position)
            };
        }

        /// <summary>
        /// Parses an assignment statement: identifier = expression
        /// </summary>
        private Ast.Assign ParseAssignment()
        {
            var identifier = ParseIdentifier();

            if (_current.Type != TokenType.Assign)
            {
                throw new InvalidOperationException($"Expected assignment token, but current token type is {_current.Type}");
            }
            MoveNext(); // consume '='

            var expression = ParseExpression();
            return new Ast.Assign(identifier, expression, identifier.Position);
        }

        /// <summary>
        /// Parses an output statement: << expression
        /// </summary>
        private Ast.Output ParseOutput()
        {
            var position = _current.Position;
            
            if (_current.Type != TokenType.Output)
            {
                throw new InvalidOperationException($"Expected output token, but current token type is {_current.Type}");
            }
            MoveNext(); // consume '<<'

            var expression = ParseExpression();
            return new Ast.Output(expression, position);
        }

        private Ast.Identifier ParseIdentifier()
        {
            var token = _current;

            if (token.Type != TokenType.Identifier)
            {
                throw new InvalidOperationException($"Expected identifier token, but current token type is {_current.Type}");
            }

            MoveNext(); // consume identifier
            return new Ast.Identifier(token.Value, token.Position);
        }

        /// <summary>
        /// Parses an expression (for now, just literals)
        /// </summary>
        private Ast.Expression ParseExpression()
        {
            var token = _current;
            MoveNext();

            return token.Type switch
            {
                TokenType.Integer => new Ast.Number(int.Parse(token.Value), token.Position),
                TokenType.String => new Ast.String(token.Value, token.Position),
                TokenType.Identifier => new Ast.Variable(token.Value, token.Position),
                _ => throw new InvalidSyntaxException(token.Position)
            };
        }

        /// <summary>
        /// Moves to the next token
        /// </summary>
        private void MoveNext()
        {
            _current = _next;
            _next = _tokens.MoveNext() ? _tokens.Current : new Token(TokenType.EndOfFile, string.Empty, _current.Position);
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
