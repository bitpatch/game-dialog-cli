# GitHub Copilot Instructions

## Language Guidelines

- **All commit messages must be written in English**
- **All code comments must be written in English**
- This ensures consistency and maintainability across the codebase for all contributors

## Project Structure

### DialogLang (Paused Development)

The **DialogLang** project contains the initial development of the Game Dialog Script Language interpreter. Development on this project is **temporarily paused**.

### DialogLangExt (Active Development)

The **DialogLangExt** project is the new, actively developed version of the Game Dialog Script Language interpreter. All current script interpreter development should focus on this project.

#### Interpreter Structure

The interpreter follows a classic three-stage pipeline architecture with streaming at each stage:

1. **Lexer** (`Lexer.cs`) - Tokenization
   - Input: `TextReader` with source code
   - Output: `IEnumerable<Token>` (streaming)
   - Converts source text into tokens using `yield return`
   - Token types: `Identifier`, `Number`, `String`, `True`, `False`, `Assign` (=), `Output` (<<), `And`, `Or`, `Not`, `Xor`, `LeftParen` (, `RightParen` ), `Newline`, `EndOfFile`
   - Keywords: `true`, `false`, `and`, `or`, `not`, `xor`

2. **Parser** (`Parser.cs`) - Syntax Analysis
   - Input: `IEnumerable<Token>` from Lexer
   - Output: `IEnumerable<Ast.Statement>` (streaming)
   - Builds AST nodes using `yield return`
   - Uses one-token lookahead (`_current` and `_next`)
   - Statement types: `Assign`, `Output`
   - Expression parsing with operator precedence (low to high): `or`, `xor`, `and`, `not`, primary
   - Supports parenthesized expressions

3. **Interpreter** (`Interpreter.cs`) - Execution
   - Input: `IEnumerable<Ast.Statement>` from Parser
   - Output: `IEnumerable<object>` (streaming)
   - Executes statements and yields output values from `<<` statements
   - Maintains variable scope in `Dictionary<string, object>`

4. **Public API** (`Dialog.cs`)
   - `Execute(TextReader)` and `Execute(string)` methods
   - Chains all three stages together
   - Returns `IEnumerable<object>` with output values

**AST Nodes** (`Ast/Nodes.cs`):
- Base: `Node`, `Statement`, `Expression`, `Value`
- Values: `Number`, `String`, `Boolean`, `Variable`
- Operations: `AndOp`, `OrOp`, `XorOp`, `NotOp`
- Statements: `Assign`, `Output`
- Root: `Program`

All nodes are immutable records with `TokenPosition` for error reporting.

#### Streaming Architecture Instructions

- Interpreter, parser, and lexer must operate in streaming mode.
- Data should be passed between components using `yield` (C# iterator pattern).
- Avoid buffering entire input; process tokens, AST nodes, and dialog lines as streams.
- Use `IEnumerable<T>` and `yield return` for all stages: lexing, parsing, and interpreting.
- Ensure each stage can consume and produce data incrementally.
- Document streaming behavior in code comments.

#### Nullable Reference Types

- **TreatWarningsAsErrors** is enabled - all nullable warnings are compilation errors
- **Internal/private classes and methods**: Do NOT add null checks for non-nullable parameters - the compiler enforces null safety at compile time
- **Public API classes and methods** (including public constructors and methods of internal classes): ALWAYS add explicit null checks with `ArgumentNullException` for non-nullable parameters, as external consumers may have different nullable settings or use reflection
- Example:
  ```csharp
  // Public API class - add null checks on public methods
  public class Interpreter
  {
      public void Execute(TextReader reader)
      {
          if (reader == null)
              throw new ArgumentNullException(nameof(reader));
          // ...
      }
  }
  
  // Internal class - no null checks needed anywhere
  internal class Parser
  {
      public Parser(IEnumerable<Token> tokens)
      {
          // No null check - compiler ensures tokens is not null
          _tokens = tokens.GetEnumerator();
      }
      
      private void ProcessToken(Token token)
      {
          // No null check - compiler ensures token is not null
      }
  }
  ```

## Code Style

- Follow C# naming conventions
- Keep code clean and well-documented
- Write meaningful variable and function names
