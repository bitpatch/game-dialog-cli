using BitPatch.DialogLang;

namespace GDialog;

/// <summary>
/// Handles execution of Game Dialog Script files.
/// </summary>
internal static class ScriptRunner
{
    /// <summary>
    /// Executes a Game Dialog Script file.
    /// </summary>
    /// <param name="scriptPath">Path to the script file to execute.</param>
    /// <param name="showVariables">Whether to display variables after execution.</param>
    /// <returns>Exit code: 0 for success, 1 for errors.</returns>
    public static int Run(string scriptPath, bool showVariables)
    {
        if (!File.Exists(scriptPath))
        {
            Console.Error.WriteLine($"Error: File '{scriptPath}' not found.");
            return 1;
        }

        var dialog = new Dialog();

        try
        {
            foreach (var runtimeItem in dialog.RunFile(scriptPath))
            {
                switch (runtimeItem)
                {
                    case RuntimeValue value:
                        Console.WriteLine(value);
                        break;

                    case RuntimeValueRequest request:
                        Console.Write("> ");
                        var input = Console.ReadLine() ?? "";
                        request.Request(TypeParser.ParseValue(input));
                        break;

                    default:
                        Console.Error.WriteLine($"Unknown runtime item: {runtimeItem.GetType().Name}");
                        break;
                }
            }

            if (showVariables)
            {
                Console.WriteLine("\nVariables:");
                foreach (var (name, value) in dialog.Variables)
                {
                    Console.WriteLine($"  {name} = {value}");
                }
            }

            return 0;
        }
        catch (ScriptError ex)
        {
            Console.Error.WriteLine($"---");
            Console.Error.WriteLine(LogUtils.FormatError(ex));

            if (showVariables)
            {
                Console.WriteLine("\nVariables:");
                foreach (var (name, value) in dialog.Variables)
                {
                    Console.WriteLine($"  {name} = {value}");
                }
            }

            return 1;
        }
    }
}
