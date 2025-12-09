# GitHub Copilot Instructions

## Language Guidelines

- **All commit messages must be written in English**
- **All code comments must be written in English**
- This ensures consistency and maintainability across the codebase for all contributors

## Project Overview

This repository contains the **Game Dialog Script Language** - a simple language for writing game dialogs with integrated logic. The project consists of:

### DialogLang (Core Library)

The main interpreter implementation for Game Dialog Script Language (.gds files).

- **Namespace**: `BitPatch.DialogLang`
- **Target Framework**: `netstandard2.1` (for Unity and Godot compatibility)
- **C# Version**: 9.0
- **Architecture**: Classic three-stage streaming pipeline: `Lexer → Parser → Interpreter`
- **Key Classes**:
  - `Dialog` - Public API entry point (only public class)
  - `Lexer` - Tokenizes source code (internal)
  - `Parser` - Builds AST from tokens (internal)
  - `Interpreter` - Executes AST statements (internal)
  - `Ast.Nodes` - AST node definitions using C# records
- **Unity Integration**: Includes `GameDialogScript.asmdef` for Unity compatibility

### cli (CLI Tool)

Command-line tool for running .gds script files.

- **Command**: `gdialog script.gds`
- **Target Framework**: `net10.0`
- **Package**: Published as .NET global tool (`gdialog`)
- **Distribution**: NuGet global tool + Homebrew (with special `Homebrew` build configuration)
- **Key Files**:
  - `Program.cs` - Minimal entry point using switch expressions
  - `ScriptExecutor.cs` - Script execution with detailed error reporting
  - `CommandLineOptions.cs` - CLI argument parsing

### lang.tests

Unit tests for the interpreter using xUnit, with `Utils.Execute()` helper for streamlined testing.

## Architecture Guidelines

### Streaming Architecture

All three stages (Lexer, Parser, Interpreter) MUST operate in streaming mode:

- Data flows through pipeline using `IEnumerable<T>` and `yield return`
- **No buffering** - process tokens, AST nodes, and dialog lines incrementally
- Example: `Dialog.Execute(reader)` streams from `TextReader → Lexer → Parser → Interpreter`
- Each stage yields items one by one (tokens, statements, output values)
- This enables processing large scripts without loading everything into memory

### Error Handling Pattern

- `ScriptException` base class with `Location` tracking (line/column info)
- Specific exception types: `InvalidSyntaxException`, `TypeMismatchException`
- CLI shows errors with source context: line highlighting and column markers
- Use `ScriptExecutor.PrintScriptError()` pattern for user-friendly error display

### Nullable Reference Types

- **TreatWarningsAsErrors** is enabled - all nullable warnings are compilation errors
- **Internal/private classes and methods**: Do NOT add null checks for non-nullable parameters - the compiler enforces null safety at compile time
- **Public API classes and methods** (including public constructors and methods of internal classes): ALWAYS add explicit null checks with `ArgumentNullException` for non-nullable parameters, as external consumers may have different nullable settings or use reflection

### AST Design

- All AST nodes are C# records inheriting from `Node(Location)`
- Two main categories: `Statement` (executed) and `Expression` (evaluated)
- Use `IBoolean` interface to mark expressions that can be used in conditionals

### Language Features

The Game Dialog Script Language supports:

- **Variables**: Assignment and usage (`name = "Arthur"`)
- **Output**: Dialog lines (`<< "Hello!"`)
- **String concatenation**: (`"Hello, " + name + "!"`)
- **Arithmetic**: `+`, `-`, `*`, `/`, `%`
- **Comparisons**: `==`, `!=`, `<`, `>`, `<=`, `>=`
- **Boolean logic**: `and`, `or`, `not`, `true`, `false`
- **Control flow**: `if`/`else`, `while` loops
- **Indentation-based blocks**: Spaces or tabs (consistent style enforced)

## Development Workflows

### Building and Testing

- **Build**: Use VS Code's default build task or `dotnet build cli/cli.csproj`
- **Tests**: Run all tests with `dotnet test lang.tests/`
- **Local Testing**: Use `Utils.Execute("script")` in tests for quick execution
- **CLI Testing**: Build then run `dotnet run --project cli script.gds`

### Distribution Builds

- **NuGet Global Tool**: Standard `Release` configuration publishes to NuGet
- **Homebrew**: Special `Homebrew` configuration creates self-contained ARM64 binary
  - Uses `RuntimeIdentifier=osx-arm64` and `PublishSingleFile=true`
  - Automatically creates `.tar.gz` archive via MSBuild target
- Both disable debug symbols (`DebugType=none`) for smaller packages

### Testing Patterns

- Use `Utils.Execute()` helper: `var output = Utils.Execute("x = 1\n<< x");`
- Test files typically verify both output and final variable state
- Lexer tests use `Tokenize()` helper to convert source to token list
- Follow pattern: Arrange (script string) → Act (Execute) → Assert (output + variables)

## Code Style

- Follow C# naming conventions
- All internal types and implementation details should be `internal`
- Public API surface is minimal (only `Dialog` class)
- Keep code clean and well-documented with XML comments
- Write meaningful variable and function names
- Use explicit types for clarity where appropriate
