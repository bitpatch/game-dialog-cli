namespace BitPatch.DialogLang
{
    /// <summary>
    /// Token types for the Game Dialog Script language
    /// </summary>
    internal enum TokenType
    {
        // Literals
        Integer,        // 123, 456
        Identifier,     // variable names
        
        // Operators
        Assign,         // =
        
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
        public int Line { get; }
        public int Column { get; }

        public Token(TokenType type, string value, int line, int column)
        {
            Type = type;
            Value = value;
            Line = line;
            Column = column;
        }

        public override string ToString()
        {
            return $"Token({Type}, '{Value}', Line: {Line}, Col: {Column})";
        }
    }
}
