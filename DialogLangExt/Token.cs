namespace BitPatch.DialogLang
{
    /// <summary>
    /// Token types for the Game Dialog Script language
    /// </summary>
    internal enum TokenType
    {
        // Literals
        Integer,        // 123, 456
        Float,          // 3.14, 2.5
        String,         // "Hello World"
        True,           // true
        False,          // false
        Identifier,     // variable names
        
        // Operators
        Assign,         // =
        Output,         // <<
        And,            // and
        Or,             // or
        Not,            // not
        Xor,            // xor
        Plus,           // +
        Minus,          // -
        Multiply,       // *
        Divide,         // /
        Modulo,         // %
        
        // Comparison operators
        GreaterThan,    // >
        LessThan,       // <
        GreaterOrEqual, // >=
        LessOrEqual,    // <=
        Equal,          // ==
        NotEqual,       // !=
        
        // Delimiters
        LeftParen,      // (
        RightParen,     // )
        
        // Indentation
        Indent,         // Increase in indentation level
        Dedent,         // Decrease in indentation level
        
        // Special
        Newline,        // End of line (statement terminator)
        EndOfFile
    }

    /// <summary>
    /// Represents a single token in the source code
    /// </summary>
    internal class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public Location Location { get; }

        public Token(TokenType type, string value, int line, int initial, int final)
            : this(type, value, new Location(line, initial, final))
        {
        }

        public Token(TokenType type, string value, int line, int position)
            : this(type, value, new Location(line, position))
        {
        }

        public Token(TokenType type, string value, Location location)
        {
            Type = type;
            Value = value;
            Location = location;
        }

        public bool IsEndOfFile()
        {
            return Type is TokenType.EndOfFile;
        }

        public bool IsEndOfStatement()
        {
            return Type is TokenType.Newline or TokenType.EndOfFile;
        }

        public static Token EndOfFile(Location location)
        {
            return new Token(TokenType.EndOfFile, string.Empty, location);
        }

        public static Token EndOfFile(int line, int column)
        {
            return EndOfFile(new Location(line, column));
        }

        public static Token NewLine(int line, int column)
        {
            return new Token(TokenType.Newline, string.Empty, new Location(line, column));
        }

        public static Token Indent(int line, int column)
        {
            return new Token(TokenType.Indent, string.Empty, line, column);
        }

        public static Token Dedent(int line, int column)
        {
            return new Token(TokenType.Dedent, string.Empty, line, column);
        }

        public override string ToString()
        {
            return $"Token({Type}, '{Value}', {Location})";
        }
    }
}
