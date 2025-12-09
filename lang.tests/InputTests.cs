using BitPatch.DialogLang;

namespace Lang.Tests;

public class InputTests
{
    /// <summary>
    /// Executes the script and fulfills input requests with the provided values.
    /// </summary>
    private static List<RuntimeItem> ExecuteWithInput(string script, params string[] inputValues)
    {
        var dialog = new Dialog();
        var result = new List<RuntimeItem>();
        var inputIndex = 0;

        foreach (var item in dialog.RunInline(script))
        {
            switch (item)
            {
                case RuntimeValueRequest request:
                    if (inputIndex >= inputValues.Length)
                    {
                        throw new InvalidOperationException("Not enough input values provided for the script.");
                    }
                    request.Request(inputValues[inputIndex++]);
                    break;

                case RuntimeValue value:
                    result.Add(value);
                    break;
            }
        }

        return result;
    }

    /// <summary>
    /// Executes the script and returns the dialog instance for variable inspection.
    /// </summary>
    private static Dialog ExecuteDialogWithInput(string script, params string[] inputValues)
    {
        var dialog = new Dialog();
        var inputIndex = 0;

        foreach (var item in dialog.RunInline(script))
        {
            if (item is RuntimeValueRequest request)
            {
                if (inputIndex >= inputValues.Length)
                {
                    throw new InvalidOperationException("Not enough input values provided for the script.");
                }
                request.Request(inputValues[inputIndex++]);
            }
        }

        return dialog;
    }

    [Fact]
    public void InputStoresValueInVariable()
    {
        // Arrange
        var script = ">> name";

        // Act
        var dialog = ExecuteDialogWithInput(script, "Alice");

        // Assert
        var nameVar = dialog.Variables.FirstOrDefault(v => v.name == "name");
        Assert.NotNull(nameVar.value);
        Assert.IsType<RuntimeString>(nameVar.value);
        Assert.Equal("Alice", ((RuntimeString)nameVar.value).Value);
    }

    [Fact]
    public void InputValueCanBeUsedInOutput()
    {
        // Arrange
        var script = """
            >> name
            << "Hello, " + name
            """;

        // Act
        var result = ExecuteWithInput(script, "Bob");

        // Assert
        result.AssertEqual("Hello, Bob");
    }

    [Fact]
    public void MultipleInputStatements()
    {
        // Arrange
        var script = """
            >> first
            >> second
            << first + " " + second
            """;

        // Act
        var result = ExecuteWithInput(script, "Hello", "World");

        // Assert
        result.AssertEqual("Hello World");
    }

    [Fact]
    public void InputWithConditional()
    {
        // Arrange
        var script = """
            >> answer
            if answer == "yes"
                << "Confirmed"
            """;

        // Act
        var result = ExecuteWithInput(script, "yes");

        // Assert
        result.AssertEqual("Confirmed");
    }

    [Fact]
    public void InputWithConditionalElse()
    {
        // Arrange
        var script = """
            >> answer
            if answer == "yes"
                << "Confirmed"
            else
                << "Denied"
            """;

        // Act
        var result = ExecuteWithInput(script, "no");

        // Assert
        result.AssertEqual("Denied");
    }

    [Fact]
    public void InputYieldsRuntimeValueRequest()
    {
        // Arrange
        var script = ">> name";
        var dialog = new Dialog();

        // Act - iterate manually to avoid forcing full enumeration
        var enumerator = dialog.RunInline(script).GetEnumerator();
        Assert.True(enumerator.MoveNext());

        // Assert - first item should be RuntimeValueRequest
        Assert.IsType<RuntimeValueRequest>(enumerator.Current);
    }
}
