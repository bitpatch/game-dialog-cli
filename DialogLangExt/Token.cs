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
        EndOfFile,
        Unknown
    }

    /// <summary>
    /// Represents a single token in the source code
    /// </summary>
    internal class Token
    {
        public TokenType Type { get; }
        public string Value { get; }
        public int Position { get; }

        public Token(TokenType type, string value, int position)
        {
            Type = type;
            Value = value;
            Position = position;
        }

        public override string ToString()
        {
            return $"Token({Type}, '{Value}', {Position})";
        }
    }
}
