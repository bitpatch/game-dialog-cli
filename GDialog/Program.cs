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
    "--about" => CommandLineOptions.ShowAbout(),
    string arg when arg.StartsWith('-') => CommandLineOptions.ShowUnknownOption(arg),
    _ => RunScript(args)
};

static int RunScript(string[] args)
{
    var scriptPath = args[0];
    var showVariables = args.Length > 1 && (args[1] == "--vars" || args[1] == "-v");
    return ScriptRunner.Run(scriptPath, showVariables);
}

