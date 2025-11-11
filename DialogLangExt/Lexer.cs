using System;
using System.Collections.Generic;
using System.Text;

namespace BitPatch.DialogLang
{
    /// <summary>
    /// Lexical analyzer that converts source code into tokens
    /// </summary>
    internal class Lexer
    {
        private readonly string _source;
        private int _position;

        public Lexer(string source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _position = 0;
        }

        /// <summary>
        /// Tokenizes the entire source code
        /// </summary>
        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();

            while (_position < _source.Length)
            {
                var token = GetNextToken();
                if (token.Type != TokenType.Unknown)
                {
                    tokens.Add(token);
                }
            }

            tokens.Add(new Token(TokenType.EndOfFile, string.Empty, _position));
            return tokens;
        }

        /// <summary>
        /// Gets the next token from the source code
        /// </summary>
        private Token GetNextToken()
        {
            // Skip whitespace
            while (_position < _source.Length && char.IsWhiteSpace(_source[_position]))
            {
                _position++;
            }

            if (_position >= _source.Length)
            {
                return new Token(TokenType.EndOfFile, string.Empty, _position);
            }

            var currentChar = _source[_position];
            var startPosition = _position;

            // Integer number
            if (char.IsDigit(currentChar))
            {
                return ReadNumber(startPosition);
            }

            // Identifier (variable name)
            if (char.IsLetter(currentChar) || currentChar == '_')
            {
                return ReadIdentifier(startPosition);
            }

            // Assignment operator
            if (currentChar == '=')
            {
                _position++;
                return new Token(TokenType.Assign, "=", startPosition);
            }

            // Unknown character - skip it
            _position++;
            return new Token(TokenType.Unknown, currentChar.ToString(), startPosition);
        }

        /// <summary>
        /// Reads an integer number from the source
        /// </summary>
        private Token ReadNumber(int startPosition)
        {
            var sb = new StringBuilder();

            while (_position < _source.Length && char.IsDigit(_source[_position]))
            {
                sb.Append(_source[_position]);
                _position++;
            }

            return new Token(TokenType.Integer, sb.ToString(), startPosition);
        }

        /// <summary>
        /// Reads an identifier (variable name) from the source
        /// </summary>
        private Token ReadIdentifier(int startPosition)
        {
            var sb = new StringBuilder();

            while (_position < _source.Length && 
                   (char.IsLetterOrDigit(_source[_position]) || _source[_position] == '_'))
            {
                sb.Append(_source[_position]);
                _position++;
            }

            return new Token(TokenType.Identifier, sb.ToString(), startPosition);
        }
    }
}
