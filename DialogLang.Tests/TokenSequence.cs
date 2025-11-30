using BitPatch.DialogLang;
using Xunit.Abstractions;

namespace DialogLang.Tests;

/// <summary>
/// Represents a sequence of tokens for testing purposes.
/// </summary>
public class TokenSequence : IXunitSerializable
{
    /// <summary>
    /// The sequence of token types.
    /// </summary>
    internal TokenType[] Sequence { get; private set; } = [];

    /// <summary>
    /// Required for deserialization.
    /// </summary>
    public TokenSequence() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenSequence"/> class with the specified token types.
    /// </summary>
    internal TokenSequence(TokenType[] sequence)
    {
        Sequence = sequence;
    }

    /// <summary>
    /// Deserializes the token sequence from the given serialization info.
    /// </summary>
    public void Deserialize(IXunitSerializationInfo info)
    {
        var values = info.GetValue<int[]>("tokens");
        Sequence = [..values.Select(v => (TokenType)v)];
    }

    /// <summary>
    /// Serializes the token sequence to the given serialization info.
    /// </summary>
    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue("tokens", Sequence.Select(t => (int)t).ToArray());
    }

    /// <summary>
    /// Returns a string representation of the token sequence.
    /// </summary>
    public override string ToString()
    {
        return string.Join(", ", Sequence);
    }
}
