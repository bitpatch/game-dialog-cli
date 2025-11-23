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

            foreach (var output in dialog.RunFile(scriptPath))
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
            Console.WriteLine(LogUtils.FormatError(ex));
            return 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return 1;
        }
    }
}
