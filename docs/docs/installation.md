---
sidebar_position: 2
---

# Installation

There are several ways to install the Game Dialog Script CLI tool (`gdialog`).

## NuGet (Global .NET Tool)

The recommended way to install `gdialog` if you have .NET SDK installed:

```bash
dotnet tool install -g gdialog
```

To update to the latest version:

```bash
dotnet tool update -g gdialog
```

## Homebrew (macOS)

Install via Homebrew:

```bash
brew tap bitpatch/tools
brew install gdialog
```

## Verify Installation

After installation, verify it works:

```bash
gdialog --version
```

## Requirements

- **.NET Global Tool**: Requires .NET 8.0 SDK or later
- **Homebrew**: macOS with Apple Silicon (ARM64)

## Using the Library

If you want to integrate Game Dialog Script into your game engine, add the NuGet package to your project:

```bash
dotnet add package DialogLang
```

Or add it to your `.csproj`:

```xml
<PackageReference Include="DialogLang" Version="*" />
```

### Unity Integration

For Unity projects, the library includes an assembly definition file (`GameDialogScript.asmdef`) for proper Unity integration. Simply copy the `DialogLang` folder into your Unity project's `Assets` directory.

### Godot Integration

For Godot with C#, add the NuGet package to your project and use the `Dialog` class directly in your scripts.
