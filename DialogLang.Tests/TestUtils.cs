using BitPatch.DialogLang;

namespace DialogLang.Tests;

internal static class Utils
{
    public static List<object> Execute(string script)
    {
        var dialog = new Dialog();
        return [.. dialog.Execute(script)];
    }
}