using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BitPatch.DialogLang
{
    /// <summary>
    /// Lexical analyzer that converts source code into tokens
    /// </summary>
    internal class Lexer
    {
        private readonly TextReader _reader;
        private readonly StringBuilder _buffer;
        private int _current;
        private int _position;

        public Lexer(TextReader reader)
        {
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _buffer = new StringBuilder();
            _position = 0;
            _current = _reader.Read();
        }

        /// <summary>
        /// Tokenizes the source code one token at a time (streaming)
        /// </summary>
        public IEnumerable<Token> Tokenize()
        {
            while (_current != -1)
            {
                var token = GetNextToken();

                if (token.Type != TokenType.Unknown)
                {
                    yield return token;
                }
            }

            yield return new Token(TokenType.EndOfFile, string.Empty, _position);
        }

        /// <summary>
        /// Gets the next token from the source code
        /// </summary>
        private Token GetNextToken()
        {
            if (_current == -1)
            {
                return new Token(TokenType.EndOfFile, string.Empty, _position);
            }

            // Skip whitespace
            while (char.IsWhiteSpace((char)_current))
            {
                MoveNextChar();
            }

            var currentChar = (char)_current;
            var startPosition = _position;

            return currentChar switch
            {
                // Integer number
                >= '0' and <= '9' => ReadNumber(startPosition),

                // Identifier (variable name) 
                >= 'a' and <= 'z' => ReadIdentifier(startPosition),
                >= 'A' and <= 'Z' => ReadIdentifier(startPosition),
                '_' => ReadIdentifier(startPosition),

                // Single-character operators
                '=' => CreateSingleCharToken(TokenType.Assign, "=", startPosition),

                // Unknown character
                _ => CreateSingleCharToken(TokenType.Unknown, currentChar.ToString(), startPosition)
            };
        }

        /// <summary>
        /// Creates a single-character token and advances position
        /// </summary>
        private Token CreateSingleCharToken(TokenType type, string value, int position)
        {
            MoveNextChar();
            return new Token(type, value, position);
        }

        /// <summary>
        /// Reads an integer number from the source
        /// </summary>
        private Token ReadNumber(int startPosition)
        {
            _buffer.Clear();

            while (_current != -1 && char.IsDigit((char)_current))
            {
                _buffer.Append((char)_current);
                MoveNextChar();
            }

            return new Token(TokenType.Integer, _buffer.ToString(), startPosition);
        }

        /// <summary>
        /// Reads an identifier (variable name) from the source
        /// </summary>
        private Token ReadIdentifier(int startPosition)
        {
            _buffer.Clear();

            while (_current != -1 && (char.IsLetterOrDigit((char)_current) || (char)_current == '_'))
            {
                _buffer.Append((char)_current);
                MoveNextChar();
            }

            return new Token(TokenType.Identifier, _buffer.ToString(), startPosition);
        }

        /// <summary>
        /// Advances to the next character
        /// </summary>
        private void MoveNextChar()
        {
            _current = _reader.Read();
            _position++;
        }
    }
}
