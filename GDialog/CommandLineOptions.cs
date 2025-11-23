using System.IO.Compression;
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
        Console.WriteLine("gdialog - 'Game Dialog Script' CLI tool");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  gdialog <script.gds> [--vars] Run a script file");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  -h, --help                    Display this help message and exit");
        Console.WriteLine("  -v, --version                 Display version information and exit");
        Console.WriteLine("  --about                       Show information about 'Game Dialog Script' language");
        Console.WriteLine();
        Console.WriteLine("Script running options:");
        Console.WriteLine("  -v, --vars                    Display variables after script execution");
        Console.WriteLine("                                (used as: gdialog script.gds -v)");
        Console.WriteLine();
        Console.WriteLine("For more information, visit: https://github.com/bitpatch/game-dialog-cli");
        return 0;
    }

    public static int ShowAbout()
    {
        Console.WriteLine("---------------------------");
        Console.WriteLine();
        Console.WriteLine("Game Dialog Script is a simple and accessible language for");
        Console.WriteLine("creating branched dialogues in games. Thanks to the fact that");
        Console.WriteLine("is fully implemented in C#, it is designed for seamless integration");
        Console.WriteLine("with game engines such as Unity and Godot or any applications");
        Console.WriteLine("written in C#.");
        Console.WriteLine();
        Console.WriteLine("For more information, visit: https://github.com/bitpatch/game-dialog-lang");
        Console.WriteLine();
        Console.WriteLine("---------------------------");
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
        Console.Error.WriteLine($"Error: Unknown option '{option}'");
        Console.Error.WriteLine("Use 'gdialog --help' for usage information.");
        return 2;
    }
}
