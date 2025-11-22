# Game Dialog Script CLI

[![GitHub Release](https://img.shields.io/github/v/release/bitpatch/game-dialog-cli)](https://github.com/bitpatch/game-dialog-cli/releases)
[![NuGet](https://img.shields.io/nuget/v/gdialog.svg)](https://www.nuget.org/packages/gdialog/)
[![Homebrew](https://img.shields.io/badge/homebrew-bitpatch%2Ftools-orange)](https://github.com/bitpatch/homebrew-tools)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A simple command-line tool for running Game Dialog Script Language files.

## What It Does

Game Dialog Script CLI is an interpreter that executes `.gds` (Game Dialog Script) files. It reads your dialog scripts and outputs the results directly to the console, making it easy to test and debug your game dialogs without running the full game.

## Installation

### NuGet (Global .NET Tool)

Install `gdialog` as a global .NET tool:

```bash
dotnet tool install -g gdialog
```

### Homebrew

Install via Homebrew:

```bash
brew tap bitpatch/tools
brew install gdialog
```

## Usage

Run a dialog script:

```bash
gdialog script.gds
```

The tool will execute the script and display:
- Dialog lines (marked with `<<`)
- Final state of all variables

## Example

Given a script file `greeting.gds`:

```python
playerName = "Arthur"
reputation = 75

<< "Welcome, traveler!"

if reputation > 50
    << "Good to see you again, " + playerName + "!"
else
    << "I don't know you."
```

Run it:

```bash
gdialog greeting.gds
```

Output:

```
Welcome, traveler!
Good to see you again, Arthur!

Variables:
  playerName = Arthur
  reputation = 75
```
