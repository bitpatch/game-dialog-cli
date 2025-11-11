using System.Linq;
using System.Text;
using BitPatch.DialogLang;

if (args.Length == 0)
{
    Console.WriteLine("Usage: dialog <script.gds>");
    return 1;
}

string scriptPath = args[0];

if (!File.Exists(scriptPath))
{
    Console.WriteLine($"Error: File '{scriptPath}' not found.");
    return 1;
}

try
{
    var dialog = new Dialog();

    // Use streaming from file - no need to load entire file into memory
    using var fileStream = File.OpenRead(scriptPath);
    using var reader = new StreamReader(fileStream);

    dialog.Execute(reader);

    Console.WriteLine("Script executed successfully.");
    Console.WriteLine("\nVariables:");
    foreach (var variable in dialog.Variables)
    {
        Console.WriteLine($"  {variable.Key} = {variable.Value}");
    }

    return 0;
}
catch (ScriptException ex)
{
    Console.WriteLine($"Error {ex.Message} (line {ex.Line}, column {ex.Column})");
    PrintScriptError(scriptPath, ex.Line, ex.Column);
    return 1;
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    return 1;
}

static void PrintScriptError(string scriptPath, int line, int column)
{
    try
    {
        if (line <= 0)
        {
            Console.WriteLine("    <unknown location>");
            return;
        }

        string? errorLine = File.ReadLines(scriptPath).Skip(line - 1).FirstOrDefault();
        if (errorLine == null)
        {
            Console.WriteLine("    <line unavailable>");
            return;
        }

        string prefix = $"    Line {line}: ";
        Console.WriteLine(prefix + errorLine);

        int pointerColumn = Math.Max(column, 1);
        var pointerBuilder = new StringBuilder();
        pointerBuilder.Append(' ', prefix.Length);
        for (int i = 1; i < pointerColumn; i++)
        {
            if (i <= errorLine.Length && errorLine[i - 1] == '\t')
            {
                pointerBuilder.Append('\t');
            }
            else
            {
                pointerBuilder.Append(' ');
            }
        }

        pointerBuilder.Append('^');
        Console.WriteLine(pointerBuilder.ToString());
    }
    catch (Exception)
    {
        Console.WriteLine("    <unable to display error location>");
    }
}

