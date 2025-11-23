using BitPatch.DialogLang;

namespace DialogLang.Tests;

public class ErrorFormatTests
{
    [Fact]
    public void ShowsErrorLine()
    {
        // Arrange
        var source = """
        reputation = "hi"
        if reputation > 5
            << "Hello!"
        """;

        ScriptException? caughtException = null;
        try
        {
            Utils.Execute(source);
        }
        catch (ScriptException ex)
        {
            caughtException = ex;
        }

        // Assert
        Assert.NotNull(caughtException);
        
        // Act
        var formatted = LogUtils.FormatError(caughtException);

        // Assert
        Assert.Contains("if reputation > 5", formatted);
        Assert.Contains("~", formatted); // Contains underline
    }

    [Fact]
    public void IncludesUnderline()
    {
        // Arrange
        var source = "<< 10 / 0";
        
        ScriptException? caughtException = null;
        try
        {
            Utils.Execute(source);
        }
        catch (ScriptException ex)
        {
            caughtException = ex;
        }

        Assert.NotNull(caughtException);

        // Act
        var formatted = LogUtils.FormatError(caughtException);

        // Assert
        Assert.Contains("<< 10 / 0", formatted);
        Assert.Contains("~", formatted);
    }

    [Fact]
    public void ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => LogUtils.FormatError(null!));
    }

    [Fact]
    public void ForNullIndent()
    {
        // Arrange
        var source = "<< 10 / 0";
        
        ScriptException? caughtException = null;
        try
        {
            Utils.Execute(source);
        }
        catch (ScriptException ex)
        {
            caughtException = ex;
        }

        Assert.NotNull(caughtException);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => LogUtils.FormatError(caughtException, null!));
    }

    [Fact]
    public void UsesCustomIndent()
    {
        // Arrange
        var source = "<< 10 / 0";
        
        ScriptException? caughtException = null;
        try
        {
            Utils.Execute(source);
        }
        catch (ScriptException ex)
        {
            caughtException = ex;
        }

        Assert.NotNull(caughtException);

        // Act
        var formatted = LogUtils.FormatError(caughtException, ">>  ");

        // Assert
        Assert.StartsWith(">>  ", formatted);
        Assert.Contains("<< 10 / 0", formatted);
    }
}
