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

        public Token(TokenType type, string value, int line, int column)
            : this(type, value, new Location(line, column))
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
            return Type == TokenType.EndOfFile;
        }

        public bool IsEndOfStatement()
        {
            return Type == TokenType.Newline || Type == TokenType.EndOfFile;
        }

        public static Token EndOfFile(Location location)
        {
            return new Token(TokenType.EndOfFile, string.Empty, location);
        }

        public override string ToString()
        {
            return $"Token({Type}, '{Value}', {Location})";
        }
    }
}
