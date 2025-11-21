using System.Reflection;

namespace GDialog;

/// <summary>
/// Handles command-line option processing and display.
/// </summary>
internal static class CommandLineOptions
{
    /// <summary>
    /// Shows usage information when no arguments are provided.
    /// </summary>
    /// <returns>Exit code 2 (invalid usage).</returns>
    public static int ShowUsage()
    {
        Console.WriteLine("Usage: gdialog <script.gds>");
        Console.WriteLine("Try 'gdialog --help' for more information.");
        return 2;
    }

    /// <summary>
    /// Shows help information with usage, options, and examples.
    /// </summary>
    /// <returns>Exit code 0 (success).</returns>
    public static int ShowHelp()
    {
        Console.WriteLine("gdialog - Game Dialog Script Interpreter");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  gdialog <script.gds>          Execute a Game Dialog Script file");
        Console.WriteLine("  gdialog --help                Show this help message");
        Console.WriteLine("  gdialog --version             Show version information");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  -h, --help                    Display this help message and exit");
        Console.WriteLine("  -v, --version                 Display version information and exit");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  gdialog greeting.gds          Run the greeting.gds script");
        Console.WriteLine("  gdialog demo.gds              Run the demo.gds script");
        Console.WriteLine();
        Console.WriteLine("For more information, visit: https://github.com/bitpatch/game-dialog-cli");
        return 0;
    }

    /// <summary>
    /// Shows version information.
    /// </summary>
    /// <returns>Exit code 0 (success).</returns>
    public static int ShowVersion()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        var versionString = version != null 
            ? $"{version.Major}.{version.Minor}.{version.Build}" 
            : "unknown";
        Console.WriteLine($"gdialog version {versionString}");
        return 0;
    }

    /// <summary>
    /// Shows error message for unknown options.
    /// </summary>
    /// <param name="option">The unknown option that was provided.</param>
    /// <returns>Exit code 2 (invalid usage).</returns>
    public static int ShowUnknownOption(string option)
    {
        Console.WriteLine($"Error: Unknown option '{option}'");
        Console.WriteLine("Use 'gdialog --help' for usage information.");
        return 2;
    }
}
