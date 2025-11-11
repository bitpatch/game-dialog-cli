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
            if (_current == -1)
            {
                return new Token(TokenType.EndOfFile, string.Empty, _line, _column);
            }

            // Skip whitespace except newlines
            while (_current != -1 && char.IsWhiteSpace((char)_current) && (char)_current != '\n')
            {
                MoveNextChar();
            }

            var currentChar = (char)_current;
            var startLine = _line;
            var startColumn = _column;

            return currentChar switch
            {
                // Newline (statement terminator) - skip consecutive newlines
                '\n' => ReadNewline(startLine, startColumn),

                // String literal
                '"' => ReadString(startLine, startColumn),

                // Integer number
                >= '0' and <= '9' => ReadNumber(startLine, startColumn),

                // Identifier (variable name) 
                >= 'a' and <= 'z' => ReadIdentifier(startLine, startColumn),
                >= 'A' and <= 'Z' => ReadIdentifier(startLine, startColumn),
                '_' => ReadIdentifier(startLine, startColumn),

                // Single-character operators
                '=' => ReadSingleCharToken(TokenType.Assign, "=", startLine, startColumn),

                // Unknown character
                _ => throw new ScriptException($"Unknown character: '{currentChar}'", startLine, startColumn)
            };
        }

        /// <summary>
        /// Creates a single-character token and advances position
        /// </summary>
        private Token ReadSingleCharToken(TokenType type, string value, int line, int column)
        {
            MoveNextChar();
            return new Token(type, value, line, column);
        }

        /// <summary>
        /// Reads an integer number from the source
        /// </summary>
        private Token ReadNumber(int startLine, int startColumn)
        {
            _buffer.Clear();

            while (_current != -1 && char.IsDigit((char)_current))
            {
                _buffer.Append((char)_current);
                MoveNextChar();
            }

            return new Token(TokenType.Integer, _buffer.ToString(), startLine, startColumn);
        }

        /// <summary>
        /// Reads an identifier (variable name) from the source
        /// </summary>
        private Token ReadIdentifier(int startLine, int startColumn)
        {
            _buffer.Clear();

            while (_current != -1 && (char.IsLetterOrDigit((char)_current) || (char)_current == '_'))
            {
                _buffer.Append((char)_current);
                MoveNextChar();
            }

            return new Token(TokenType.Identifier, _buffer.ToString(), startLine, startColumn);
        }

        /// <summary>
        /// Reads a string literal from the source (enclosed in double quotes)
        /// </summary>
        private Token ReadString(int startLine, int startColumn)
        {
            _buffer.Clear();

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
                        throw new ScriptException("Unterminated string literal", startLine, startColumn);
                    }

                    var escapeChar = (char)_current;
                    _buffer.Append(escapeChar switch
                    {
                        'n' => '\n',
                        't' => '\t',
                        'r' => '\r',
                        '\\' => '\\',
                        '"' => '"',
                        _ => throw new ScriptException($"Invalid escape sequence: \\{escapeChar}", _line, _column)
                    });
                    MoveNextChar();
                }
                else if ((char)_current == '\n')
                {
                    throw new ScriptException("Unterminated string literal (newline in string)", _line, _column);
                }
                else
                {
                    _buffer.Append((char)_current);
                    MoveNextChar();
                }
            }

            if (_current == -1)
            {
                throw new ScriptException("Unterminated string literal", startLine, startColumn);
            }

            // Skip closing quote
            MoveNextChar();

            return new Token(TokenType.String, _buffer.ToString(), startLine, startColumn);
        }

        /// <summary>
        /// Reads a newline token, skipping consecutive newlines and whitespace between them
        /// </summary>
        private Token ReadNewline(int startLine, int startColumn)
        {
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
    }
}
