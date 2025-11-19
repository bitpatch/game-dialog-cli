static class CharExtension
{
    public static bool IsNewLine(this char c)
    {
        return c is '\n' or '\r' or '\u2028' or '\u2029' or '\u0085';
    }

    public static bool IsNewLine(this int n)
    {
        return n is not -1 && ((char)n).IsNewLine();
    }

    public static bool IsWhiteSpace(this int n)
    {
        return n is not -1 && char.IsWhiteSpace((char)n) && !((char)n).IsNewLine();
    }

    public static bool IsIdentifierChar(this int n)
    {
        if (n is -1)
        {
            return false;
        }

        var c = (char)n;
        return c is '_' || char.IsLetterOrDigit(c);
    }

    public static bool IsDigit(this int n)
    {
        return n is not -1 && char.IsDigit((char)n);
    }

    public static bool IsChar(this int n)
    {
        return n is not -1;
    }
}