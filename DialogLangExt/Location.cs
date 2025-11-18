namespace BitPatch.DialogLang
{
    /// <summary>
    /// Represents a position range in the source code
    /// </summary>
    internal readonly struct Location
    {
        /// <summary>
        /// Line number (1-based)
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// Start column (1-based)
        /// </summary>
        public int Initial { get; }

        /// <summary>
        /// End column (1-based, inclusive)
        /// </summary>
        public int Final { get; }

        public Location(int line, int position) : this(line, position, position)
        {
        }

        public Location(int line, int initial, int final)
        {
            Line = line;
            Initial = initial;
            Final = final;
        }

        /// <summary>
        /// Combines two locations into a single range spanning from the start of the first to the end of the second
        /// </summary>
        public static Location operator |(Location left, Location right)
        {
            return new Location(left.Line, left.Initial, right.Final);
        }

        public override string ToString()
        {
            return Initial == Final
                ? $"Line: {Line}, Col: {Initial}"
                : $"Line: {Line}, Col: {Initial}-{Final}";
        }
    }
}
