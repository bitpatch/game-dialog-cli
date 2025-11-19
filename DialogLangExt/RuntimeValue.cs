namespace BitPatch.DialogLang
{
    /// <summary>
    /// Represents a runtime value in the interpreter.
    /// This is a discriminated union of all possible value types.
    /// </summary>
    internal abstract record RuntimeValue
    {
        /// <summary>
        /// Converts the runtime value to its underlying object representation
        /// </summary>
        public abstract object ToObject();
    }

    /// <summary>
    /// Base class for numeric runtime values
    /// </summary>
    internal abstract record Number : RuntimeValue
    {
        public abstract bool IsNil { get; }
        public abstract float FloatValue { get; }
    }

    /// <summary>
    /// Integer runtime value
    /// </summary>
    internal sealed record Integer(int Value) : Number
    {
        public override object ToObject() => Value;
        public override bool IsNil => Value is 0;
        public override float FloatValue => Value;
        public override string ToString() => Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Float runtime value
    /// </summary>
    internal sealed record Float(float Value) : Number
    {
        public override object ToObject() => Value;
        public override bool IsNil => Value is 0.0f;
        public override float FloatValue => Value;
        public override string ToString() => Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// String runtime value
    /// </summary>
    internal sealed record String(string Value) : RuntimeValue
    {
        public override object ToObject() => Value;
        public override string ToString() => Value;
    }

    /// <summary>
    /// Boolean runtime value
    /// </summary>
    internal sealed record Boolean(bool Value) : RuntimeValue
    {
        public override object ToObject() => Value;
        public override string ToString() => Value ? "true" : "false";
    }
}
