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
        public IEnumerable<Ast.Statement> Parse()
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
        private Ast.Statement ParseStatement()
        {
            return _current.Type switch
            {
                TokenType.Identifier => ParseStatementFromIdentifier(),
                TokenType.Output => ParseOutput(),
                _ => throw new InvalidSyntaxException(_current.Position)
            };
        }

        private Ast.Statement ParseStatementFromIdentifier()
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

            Consume(TokenType.Assign); // consume '='

            var expression = ParseExpression();
            return new Ast.Assign(identifier, expression, identifier.Position);
        }

        /// <summary>
        /// Parses an output statement: << expression
        /// </summary>
        private Ast.Output ParseOutput()
        {
            var position = _current.Position;

            Consume(TokenType.Output); // consume '<<'

            var expression = ParseExpression();
            return new Ast.Output(expression, position);
        }

        private Ast.Identifier ParseIdentifier()
        {
            var token = _current;

            Consume(TokenType.Identifier); // consume identifier
            return new Ast.Identifier(token.Value, token.Position);
        }

        /// <summary>
        /// Parses an expression with operator precedence
        /// Precedence (low to high): or, xor, and, not, primary
        /// </summary>
        private Ast.Expression ParseExpression()
        {
            return ParseOrExpression();
        }

        /// <summary>
        /// Parses 'or' expression (lowest precedence)
        /// </summary>
        private Ast.Expression ParseOrExpression()
        {
            var left = ParseXorExpression();

            while (_current.Type == TokenType.Or)
            {
                var position = _current.Position;
                MoveNext(); // consume 'or'
                var right = ParseXorExpression();
                left = new Ast.OrOp(left, right, position);
            }

            return left;
        }

        /// <summary>
        /// Parses 'xor' expression
        /// </summary>
        private Ast.Expression ParseXorExpression()
        {
            var left = ParseAndExpression();

            while (_current.Type == TokenType.Xor)
            {
                var position = _current.Position;
                MoveNext(); // consume 'xor'
                var right = ParseAndExpression();
                left = new Ast.XorOp(left, right, position);
            }

            return left;
        }

        /// <summary>
        /// Parses 'and' expression
        /// </summary>
        private Ast.Expression ParseAndExpression()
        {
            var left = ParseNotExpression();

            while (_current.Type == TokenType.And)
            {
                var position = _current.Position;
                MoveNext(); // consume 'and'
                var right = ParseNotExpression();
                left = new Ast.AndOp(left, right, position);
            }

            return left;
        }

        /// <summary>
        /// Parses 'not' expression (unary operator)
        /// </summary>
        private Ast.Expression ParseNotExpression()
        {
            if (_current.Type == TokenType.Not)
            {
                var position = _current.Position;
                MoveNext(); // consume 'not'
                var operand = ParseNotExpression(); // right-associative
                return new Ast.NotOp(operand, position);
            }

            return ParsePrimaryExpression();
        }

        /// <summary>
        /// Parses primary expression (literals, variables, parenthesized expressions)
        /// </summary>
        private Ast.Expression ParsePrimaryExpression()
        {
            var token = _current;
            MoveNext();

            return token.Type switch
            {
                TokenType.Integer => new Ast.Integer(int.Parse(token.Value), token.Position),
                TokenType.Float => new Ast.Float(float.Parse(token.Value), token.Position),
                TokenType.String => new Ast.String(token.Value, token.Position),
                TokenType.True => new Ast.Boolean(true, token.Position),
                TokenType.False => new Ast.Boolean(false, token.Position),
                TokenType.Identifier => new Ast.Variable(token.Value, token.Position),
                TokenType.LeftParen => ParseParenthesizedExpression(),
                _ => throw new InvalidSyntaxException(token.Position)
            };
        }

        /// <summary>
        /// Parses a parenthesized expression: (expression)
        /// </summary>
        private Ast.Expression ParseParenthesizedExpression()
        {
            var expression = ParseExpression();

            if (_current.Type != TokenType.RightParen)
            {
                throw new InvalidSyntaxException(_current.Position);
            }

            MoveNext(); // consume ')'
            return expression;
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

        /// <summary>
        /// Consumes a token of the expected type and moves to the next token
        /// </summary>
        /// <param name="expectedType">The expected token type</param>
        /// <exception cref="InvalidOperationException">Thrown when current token doesn't match expected type</exception>
        private void Consume(TokenType expectedType)
        {
            if (_current.Type != expectedType)
            {
                throw new InvalidOperationException($"Expected {expectedType} token, but current token type is {_current.Type}");
            }

            MoveNext();
        }
    }
}
