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
  - `Dialog` - Public API entry point
  - `Lexer` - Tokenizes source code (internal)
  - `Parser` - Builds AST from tokens (internal)
  - `Interpreter` - Executes AST statements (internal)
  - `Ast.Nodes` - AST node definitions

### GDialog (CLI Tool)

Command-line tool for running .gds script files.

- **Command**: `gdialog script.gds`
- **Target Framework**: `net10.0`
- **Package**: Published as .NET global tool (`gdialog`)
- **Key Files**:
  - `Program.cs` - Entry point
  - `ScriptExecutor.cs` - Script execution logic
  - `CommandLineOptions.cs` - CLI argument parsing

### DialogLang.Tests

Unit tests for the interpreter using xUnit.

## Architecture Guidelines

### Streaming Architecture

All three stages (Lexer, Parser, Interpreter) MUST operate in streaming mode:

- Data flows through pipeline using `IEnumerable<T>` and `yield return`
- **No buffering** - process tokens, AST nodes, and dialog lines incrementally
- Lexer yields tokens one by one as they are parsed
- Parser yields AST statements one by one as tokens are consumed
- Interpreter yields output strings one by one as statements are executed
- This enables processing large scripts without loading everything into memory

### Nullable Reference Types

- **TreatWarningsAsErrors** is enabled - all nullable warnings are compilation errors
- **Internal/private classes and methods**: Do NOT add null checks for non-nullable parameters - the compiler enforces null safety at compile time
- **Public API classes and methods** (including public constructors and methods of internal classes): ALWAYS add explicit null checks with `ArgumentNullException` for non-nullable parameters, as external consumers may have different nullable settings or use reflection

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

## Code Style

- Follow C# naming conventions
- All internal types and implementation details should be `internal`
- Public API surface is minimal (only `Dialog` class)
- Keep code clean and well-documented with XML comments
- Write meaningful variable and function names
- Use explicit types for clarity where appropriate
