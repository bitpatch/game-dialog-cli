namespace BitPatch.DialogLang
{
    /// <summary>
    /// Token types for the Game Dialog Script language
    /// </summary>
    internal enum TokenType
    {
        // Literals
        Integer,        // 123, 456
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
        public TokenPosition Position { get; }
        
        public int Line => Position.Line;
        public int Column => Position.Column;

        public Token(TokenType type, string value, int line, int column)
            : this(type, value, new TokenPosition(line, column))
        {
        }

        public Token(TokenType type, string value, TokenPosition position)
        {
            Type = type;
            Value = value;
            Position = position;
        }

        public bool IsEndOfFile()
        {
            return Type == TokenType.EndOfFile;
        }

        public bool IsEndOfStatement()
        {
            return Type == TokenType.Newline || Type == TokenType.EndOfFile;
        }

        public override string ToString()
        {
            return $"Token({Type}, '{Value}', Line: {Line}, Col: {Column})";
        }
    }
}
