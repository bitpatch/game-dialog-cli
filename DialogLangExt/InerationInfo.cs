namespace BitPatch.DialogLang
{
    /// <summary>
    /// Information about loop iterations for preventing infinite loops.
    /// </summary>
    internal class IterationInfo
    {
        /// <summary>
        /// The line number where the loop is defined.
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// The number of iterations executed so far.
        /// </summary>
        public int Count { get; set; } = 0;

        public IterationInfo(int line)
        {
            Line = line;
            Count = 1;
        }
    }
}