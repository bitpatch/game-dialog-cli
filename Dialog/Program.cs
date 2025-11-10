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
    var logger = new ConsoleLogger();
    var interpreter = new Interpreter(logger);
    
    using var reader = new StreamReader(scriptPath);
    foreach (var result in interpreter.Run(reader))
    {
        if (result is Request input)
        {
            // Handle input request based on type
            if (input is RequestNumber numberInput)
            {
                while (true)
                {
                    Console.Write($"Input (number) for '{numberInput.VariableName}': ");
                    string? userInput = Console.ReadLine();
                    
                    if (double.TryParse(userInput, out double value))
                    {
                        numberInput.Set(value);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                    }
                }
            }
            else if (input is RequestString stringInput)
            {
                Console.Write($"Input (string) for '{stringInput.VariableName}': ");
                string? userInput = Console.ReadLine();
                stringInput.Set(userInput ?? "");
            }
            else if (input is RequestBool boolInput)
            {
                while (true)
                {
                    Console.Write($"Input (bool: true/false) for '{boolInput.VariableName}': ");
                    string? userInput = Console.ReadLine();
                    
                    if (bool.TryParse(userInput, out bool value))
                    {
                        boolInput.Set(value);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter 'true' or 'false'.");
                    }
                }
            }
            else if (input is RequestAny anyInput)
            {
                Console.Write($"Input for '{anyInput.VariableName}': ");
                string? userInput = Console.ReadLine();
                anyInput.Set(userInput ?? "");
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
