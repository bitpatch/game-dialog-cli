using DialogLang;

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
    string scriptText = File.ReadAllText(scriptPath);
    
    var logger = new ConsoleLogger();
    var interpreter = new Interpreter(logger);
    
    foreach (var result in interpreter.Run(scriptText))
    {
        if (result is IInputRequest input)
        {
            // Handle input request
            Console.Write("Input: ");
            string? userInput = Console.ReadLine();
            
            // Try to parse as number, otherwise treat as string
            if (double.TryParse(userInput, out double number))
            {
                input.Set(number);
            }
            else
            {
                input.Set(userInput ?? "");
            }
        }
        else if (result != null)
        {
            // Handle output
            Console.WriteLine(result);
        }
    }
    
    return 0;
}
catch (InterpreterException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    return 1;
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
    return 1;
}

// Simple console logger implementation
class ConsoleLogger : ILogger
{
    public void LogInfo(string message)
    {
        Console.WriteLine($"[Info] {message}");
    }
    
    public void LogWarning(string message)
    {
        Console.WriteLine($"[Warning] {message}");
    }
    
    public void LogError(string message)
    {
        Console.WriteLine($"[Error] {message}");
    }
}
