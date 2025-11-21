using System.Text;
using BitPatch.DialogLang;

namespace GDialog;

/// <summary>
/// Handles execution of Game Dialog Script files.
/// </summary>
internal static class ScriptExecutor
{
    /// <summary>
    /// Executes a Game Dialog Script file.
    /// </summary>
    /// <param name="scriptPath">Path to the script file to execute.</param>
    /// <returns>Exit code: 0 for success, 1 for errors.</returns>
    public static int Execute(string scriptPath)
    {
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

            foreach (var output in dialog.Execute(reader))
            {
                Console.WriteLine(output);
            }

            Console.WriteLine("\nVariables:");
            foreach (var (name, value) in dialog.Variables)
            {
                Console.WriteLine($"  {name} = {value}");
            }

            return 0;
        }
        catch (ScriptException ex)
        {
            Console.WriteLine($"{ex.Message}, line {ex.Line}");
            PrintScriptError(scriptPath, ex.Line, ex.Initial, ex.Final);
            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }

    /// <summary>
    /// Prints a script error with context from the source file.
    /// </summary>
    /// <param name="scriptPath">Path to the script file.</param>
    /// <param name="line">Line number where the error occurred.</param>
    /// <param name="startColumn">Starting column of the error.</param>
    /// <param name="endColumn">Ending column of the error.</param>
    private static void PrintScriptError(string scriptPath, int line, int startColumn, int endColumn)
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

            string prefix = "    ";
            Console.WriteLine(prefix + errorLine);

            // Build the underline with proper spacing and tabs
            var underlineBuilder = new StringBuilder();
            underlineBuilder.Append(' ', prefix.Length);
            
            // Add spaces/tabs up to the start column
            for (int i = 1; i < startColumn; i++)
            {
                if (i <= errorLine.Length && errorLine[i - 1] == '\t')
                {
                    underlineBuilder.Append('\t');
                }
                else
                {
                    underlineBuilder.Append(' ');
                }
            }

            // Underline the error range with tildes
            int underlineLength = Math.Max(1, endColumn - startColumn);
            underlineBuilder.Append('~', underlineLength);
            
            Console.WriteLine(underlineBuilder.ToString());
        }
        catch (Exception)
        {
            Console.WriteLine("    <unable to display error location>");
        }
    }
}
