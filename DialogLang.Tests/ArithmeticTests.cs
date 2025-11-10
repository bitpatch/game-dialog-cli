using System;
using System.Linq;
using DialogLang;
using Xunit;

namespace DialogLang.Tests
{
    public class ArithmeticTests
    {
        private class TestLogger : ILogger
        {
            public void LogInfo(string message) { }
            public void LogWarning(string message) { }
            public void LogError(string message) { }
        }

        private object? EvaluateExpression(string expression)
        {
            var interpreter = new Interpreter(new TestLogger());
            var script = $"<< {expression}";
            var results = interpreter.Run(script).ToList();
            return results.FirstOrDefault();
        }

        [Theory]
        [InlineData("5 + 3", 8)]
        [InlineData("10 - 4", 6)]
        [InlineData("6 * 7", 42)]
        [InlineData("20 / 5", 4)]
        [InlineData("10 + 5 - 3 * 2", 9)] // 10 + 5 - 6 = 9
        public void BasicArithmetic_ShouldReturnCorrectResult(string expression, int expected)
        {
            var result = EvaluateExpression(expression);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("2 + 3 * 4", 14)] // multiplication before addition
        [InlineData("10 - 2 * 3", 4)] // multiplication before subtraction
        [InlineData("8 / 2 + 3", 7)] // division before addition
        public void OperatorPrecedence_ShouldRespectMultiplicationAndDivisionFirst(string expression, int expected)
        {
            var result = EvaluateExpression(expression);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("(2 + 3) * 4", 20)]
        [InlineData("((10 + 5) * 2) - 6", 24)]
        [InlineData("(8 - 2) / 3", 2)]
        public void Parentheses_ShouldOverrideOperatorPrecedence(string expression, int expected)
        {
            var result = EvaluateExpression(expression);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("5.5 + 2.5", 8.0f)]
        [InlineData("10.0 / 4.0", 2.5f)]
        [InlineData("3.5 * 2.0", 7.0f)]
        public void DecimalNumbers_ShouldWorkCorrectly(string expression, float expected)
        {
            var result = EvaluateExpression(expression);
            Assert.NotNull(result);
            Assert.IsType<float>(result);
            
            var floatResult = (float)result;
            Assert.True(Math.Abs(floatResult - expected) < 0.001f, 
                $"Expected {expected}, but got {floatResult} for expression '{expression}'");
        }

        [Fact]
        public void DivisionByZero_ShouldThrowException()
        {
            Assert.Throws<DivideByZeroException>(() => EvaluateExpression("10 / 0"));
        }

        [Fact]
        public void VariableArithmetic_ShouldWorkCorrectly()
        {
            var interpreter = new Interpreter(new TestLogger());
            var script = @"a = 10
b = 20
<< a + b";
            var results = interpreter.Run(script).ToList();
            Assert.Single(results);
            Assert.Equal(30, results[0]);
        }

        [Fact]
        public void ComplexVariableExpression_ShouldWorkCorrectly()
        {
            var interpreter = new Interpreter(new TestLogger());
            var script = @"x = 5
y = 3
z = x * 2 + y
<< z";
            var results = interpreter.Run(script).ToList();
            Assert.Single(results);
            Assert.Equal(13, results[0]); // 5 * 2 + 3 = 13
        }
    }
}
