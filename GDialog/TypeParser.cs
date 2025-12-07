using System.Globalization;

namespace GDialog;

/// <summary>
/// Utility for parsing user input into appropriate types.
/// </summary>
internal static class TypeParser
{
    /// <summary>
    /// Parses a string input and returns the most appropriate typed value.
    /// Attempts to parse as int first, then float, otherwise returns the trimmed string.
    /// </summary>
    /// <param name="input">The user input string to parse.</param>
    /// <returns>An int, float, or string depending on the input format.</returns>
    public static object ParseValue(string input)
    {
        var trimmed = input.Trim();

        if (int.TryParse(trimmed, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
        {
            return intValue;
        }

        if (float.TryParse(trimmed, NumberStyles.Float, CultureInfo.InvariantCulture, out var floatValue))
        {
            return floatValue;
        }

        return trimmed;
    }
}
