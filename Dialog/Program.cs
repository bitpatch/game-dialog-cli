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
    string source = File.ReadAllText(scriptPath);
    
    dialog.Execute(source);
    
    Console.WriteLine("Script executed successfully.");
    Console.WriteLine("\nVariables:");
    foreach (var variable in dialog.Variables)
    {
        Console.WriteLine($"  {variable.Key} = {variable.Value}");
    }
    
    return 0;
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    return 1;
}

