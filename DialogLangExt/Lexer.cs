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
        private int _line;
        private int _column;

        public Lexer(TextReader reader)
        {
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _buffer = new StringBuilder();
            _line = 1;
            _column = 1;
            _current = _reader.Read();
        }

        /// <summary>
        /// Tokenizes the source code one token at a time (streaming)
        /// </summary>
        public IEnumerable<Token> Tokenize()
        {
            while (_current != -1)
            {
                yield return ReadNextToken();
            }

            yield return new Token(TokenType.EndOfFile, string.Empty, _line, _column);
        }

        /// <summary>
        /// Gets the next token from the source code
        /// </summary>
        private Token ReadNextToken()
        {
            SkipWhitespace();

            if (_current == -1)
            {
                return new Token(TokenType.EndOfFile, string.Empty, _line, _column);
            }

            return (char)_current switch
            {
                // Newline (statement terminator) - skip consecutive newlines
                '\n' => ReadNewline(),

                // String literal
                '"' => ReadString(),

                // Integer number
                >= '0' and <= '9' => ReadNumber(),

                // Identifier (variable name) 
                >= 'a' and <= 'z' => ReadIdentifier(),
                >= 'A' and <= 'Z' => ReadIdentifier(),
                '_' => ReadIdentifier(),

                // Operators
                '=' => ReadSingleCharToken(TokenType.Assign, "="),
                '<' => ReadFromLessThanSign(),

                // Unknown character
                _ => throw new InvalidSyntaxException("Unexpected symbol", _line, _column),
            };
        }

        /// <summary>
        /// Reads '<<' output operator or throws exception
        /// </summary>
        private Token ReadFromLessThanSign()
        {
            var startLine = _line;
            var startColumn = _column;
            
            MoveNextChar(); // consume first '<'
            
            if (_current == '<')
            {
                MoveNextChar(); // consume second '<'
                return new Token(TokenType.Output, "<<", startLine, startColumn);
            }
            
            throw new InvalidSyntaxException(startLine, startColumn);
        }

        /// <summary>
        /// Creates a single-character token and advances position
        /// </summary>
        private Token ReadSingleCharToken(TokenType type, string value)
        {
            var position = new TokenPosition(_line, _column);
            MoveNextChar();
            return new Token(type, value, position);
        }

        /// <summary>
        /// Reads an integer number from the source
        /// </summary>
        private Token ReadNumber()
        {
            _buffer.Clear();

            var position = new TokenPosition(_line, _column);

            while (_current != -1 && char.IsDigit((char)_current))
            {
                _buffer.Append((char)_current);
                MoveNextChar();
            }

            return new Token(TokenType.Integer, _buffer.ToString(), position);
        }

        /// <summary>
        /// Reads an identifier (variable name) from the source
        /// </summary>
        private Token ReadIdentifier()
        {
            _buffer.Clear();
            var position = new TokenPosition(_line, _column);

            while (_current != -1 && (char.IsLetterOrDigit((char)_current) || (char)_current == '_'))
            {
                _buffer.Append((char)_current);
                MoveNextChar();
            }

            return new Token(TokenType.Identifier, _buffer.ToString(), position);
        }

        /// <summary>
        /// Reads a string literal from the source (enclosed in double quotes)
        /// </summary>
        private Token ReadString()
        {
            _buffer.Clear();
            var position = new TokenPosition(_line, _column);

            // Skip opening quote
            MoveNextChar();

            while (_current != -1 && (char)_current != '"')
            {
                if ((char)_current == '\\')
                {
                    // Handle escape sequences
                    MoveNextChar();

                    if (_current == -1)
                    {
                        throw new InvalidSyntaxException(_line, _column);
                    }

                    var escapeChar = (char)_current;
                    _buffer.Append(escapeChar switch
                    {
                        'n' => '\n',
                        't' => '\t',
                        'r' => '\r',
                        '\\' => '\\',
                        '"' => '"',
                        _ => throw new InvalidSyntaxException($"Invalid escape sequence: \\{escapeChar}", _line, _column)
                    });

                    MoveNextChar();
                }
                else if ((char)_current == '\n')
                {
                    throw new InvalidSyntaxException("End of line while scanning string literal", _line, _column);
                }
                else
                {
                    _buffer.Append((char)_current);
                    MoveNextChar();
                }
            }

            if (_current == -1)
            {
                throw new InvalidSyntaxException("Unterminated string literal", _line, _column);
            }

            // Skip closing quote
            MoveNextChar();

            return new Token(TokenType.String, _buffer.ToString(), position);
        }

        /// <summary>
        /// Reads a newline token, skipping consecutive newlines and whitespace between them
        /// </summary>
        private Token ReadNewline()
        {
            var startLine = _line;
            var startColumn = _column;

            // Skip all consecutive newlines and whitespace between them
            while (_current != -1)
            {
                if ((char)_current == '\n')
                {
                    MoveNextChar();
                }
                else if (char.IsWhiteSpace((char)_current))
                {
                    // Skip whitespace between newlines
                    MoveNextChar();
                }
                else
                {
                    // Stop at non-whitespace character
                    break;
                }
            }

            return new Token(TokenType.Newline, "\n", startLine, startColumn);
        }

        /// <summary>
        /// Advances to the next character
        /// </summary>
        private void MoveNextChar()
        {
            if ((char)_current == '\n')
            {
                _line++;
                _column = 1;
            }
            else
            {
                _column++;
            }
            
            _current = _reader.Read();
        }

        /// <summary>
        /// Skips whitespace characters except newlines
        /// </summary>
        private void SkipWhitespace()
        {
            while (_current != -1 && char.IsWhiteSpace((char)_current) && (char)_current != '\n')
            {
                MoveNextChar();
            }
        }
    }
}
