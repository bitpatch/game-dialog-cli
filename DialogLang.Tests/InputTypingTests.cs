using DialogLang;
using Xunit;

namespace DialogLang.Tests
{
    public class InputTypingTests
    {
        private class TestLogger : ILogger
        {
            public void LogInfo(string message) { }
            public void LogWarning(string message) { }
            public void LogError(string message) { }
        }

        [Fact]
        public void TestInputRequestAny()
        {
            // Arrange
            var interpreter = new Interpreter(new TestLogger());
            var script = ">> x\n<< x";
            
            // Act & Assert
            var enumerator = interpreter.Run(script).GetEnumerator();
            
            // First result should be RequestAny
            Assert.True(enumerator.MoveNext());
            Assert.IsAssignableFrom<RequestAny>(enumerator.Current);
            
            var inputRequest = (RequestAny)enumerator.Current!;
            Assert.Equal("x", inputRequest.VariableName);
            
            inputRequest.Set("hello");
            
            // Next result should be the output
            Assert.True(enumerator.MoveNext());
            Assert.Equal("hello", enumerator.Current);
            
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void TestInputRequestNumber()
        {
            // Arrange
            var interpreter = new Interpreter(new TestLogger());
            var script = ">> x as number\n<< x * 2";
            
            // Act & Assert
            var enumerator = interpreter.Run(script).GetEnumerator();
            
            // First result should be RequestNumber
            Assert.True(enumerator.MoveNext());
            Assert.IsAssignableFrom<RequestNumber>(enumerator.Current);
            
            var inputRequest = (RequestNumber)enumerator.Current!;
            Assert.Equal("x", inputRequest.VariableName);
            
            // Set with a whole number - should be stored as int
            inputRequest.Set(21.0);
            
            // Next result should be the output (int * int = int)
            Assert.True(enumerator.MoveNext());
            Assert.Equal(42, enumerator.Current);
            
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void TestInputRequestNumber_WithFloat()
        {
            // Arrange
            var interpreter = new Interpreter(new TestLogger());
            var script = ">> x as number\n<< x * 2";
            
            // Act & Assert
            var enumerator = interpreter.Run(script).GetEnumerator();
            
            // First result should be RequestNumber
            Assert.True(enumerator.MoveNext());
            Assert.IsAssignableFrom<RequestNumber>(enumerator.Current);
            
            var inputRequest = (RequestNumber)enumerator.Current!;
            
            // Set with a decimal number - should be stored as float
            inputRequest.Set(21.5);
            
            // Next result should be the output (float * int = float)
            Assert.True(enumerator.MoveNext());
            Assert.Equal(43.0f, enumerator.Current);
            
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void TestInputRequestString()
        {
            // Arrange
            var interpreter = new Interpreter(new TestLogger());
            var script = ">> name as string\n<< \"Hello, \" + name";
            
            // Act & Assert
            var enumerator = interpreter.Run(script).GetEnumerator();
            
            // First result should be RequestString
            Assert.True(enumerator.MoveNext());
            Assert.IsAssignableFrom<RequestString>(enumerator.Current);
            
            var inputRequest = (RequestString)enumerator.Current!;
            Assert.Equal("name", inputRequest.VariableName);
            
            inputRequest.Set("World");
            
            // Next result should be the output
            Assert.True(enumerator.MoveNext());
            Assert.Equal("Hello, World", enumerator.Current);
            
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void TestInputRequestBool()
        {
            // Arrange
            var interpreter = new Interpreter(new TestLogger());
            var script = ">> flag as bool\n<< flag";
            
            // Act & Assert
            var enumerator = interpreter.Run(script).GetEnumerator();
            
            // First result should be RequestBool
            Assert.True(enumerator.MoveNext());
            Assert.IsAssignableFrom<RequestBool>(enumerator.Current);
            
            var inputRequest = (RequestBool)enumerator.Current!;
            Assert.Equal("flag", inputRequest.VariableName);
            
            inputRequest.Set(true);
            
            // Next result should be the output
            Assert.True(enumerator.MoveNext());
            Assert.Equal(true, enumerator.Current);
            
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void TestMultipleTypedInputs()
        {
            // Arrange
            var interpreter = new Interpreter(new TestLogger());
            var script = @"
>> age as number
>> name as string
>> active as bool
<< name + "" is "" + age + "" years old and active: "" + active
";
            
            // Act & Assert
            var enumerator = interpreter.Run(script).GetEnumerator();
            
            // First input: number
            Assert.True(enumerator.MoveNext());
            var numberInput = (RequestNumber)enumerator.Current!;
            numberInput.Set(25.0);
            
            // Second input: string
            Assert.True(enumerator.MoveNext());
            var stringInput = (RequestString)enumerator.Current!;
            stringInput.Set("Alice");
            
            // Third input: bool
            Assert.True(enumerator.MoveNext());
            var boolInput = (RequestBool)enumerator.Current!;
            boolInput.Set(true);
            
            // Output
            Assert.True(enumerator.MoveNext());
            Assert.Equal("Alice is 25 years old and active: True", enumerator.Current);
            
            Assert.False(enumerator.MoveNext());
        }
    }
}
