using GDialog;

// Handle command-line options and script execution
if (args.Length == 0)
{
    return CommandLineOptions.ShowUsage();
}

return args[0] switch
{
    "--help" or "-h" => CommandLineOptions.ShowHelp(),
    "--version" or "-v" => CommandLineOptions.ShowVersion(),
    string arg when arg.StartsWith('-') => CommandLineOptions.ShowUnknownOption(arg),
    _ => ScriptRunner.Execute(args[0])
};

