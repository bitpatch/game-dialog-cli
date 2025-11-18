using System;

namespace BitPatch.DialogLang
{
    /// <summary>
    /// Exception thrown when an error occurs during script execution.
    /// </summary>
    public class ScriptException : Exception
    {
        public int Line => Location.Line;
        public int Initial => Location.Initial;
        public int Final => Location.Final;
        
        private Location Location { get; }

        internal ScriptException(string message, Location location) : base(message)
        {
            Location = location;
        }
    }

    public class InvalidSyntaxException : ScriptException
    {
        internal InvalidSyntaxException(Location location)
            : base("Invalid syntax", location)
        {
        }

        internal InvalidSyntaxException(int line, int column)
            : this(new Location(line, column))
        {
        }

        internal InvalidSyntaxException(string message, Location location)
            : base(message, location)
        {
        }

        internal InvalidSyntaxException(string message, int line, int column)
            : base(message, new Location(line, column))
        {
        }
    }

    public class TypeMismatchException : ScriptException
    {
        internal TypeMismatchException(Type expected, RuntimeValue actual, Location location)
            : base($"Wrong type, got {actual.GetType().Name} instead of {expected.Name}", location)
        {
        }
    }
}
