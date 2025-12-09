---
sidebar_position: 1
slug: /
---

# Introduction

**Game Dialog Script** (GDS) is a simple, human-readable language designed for writing game dialogs with integrated logic. It allows game developers and writers to create interactive conversations without needing to learn complex programming languages.

## What is Game Dialog Script?

Game Dialog Script provides a clean syntax for:

- **Dialog lines** - Output text to players
- **Variables** - Store and manipulate data
- **Conditionals** - Branch dialog based on game state
- **Loops** - Repeat actions when needed
- **Expressions** - Perform calculations and comparisons

## Quick Example

Here's a simple dialog script:

```python
playerName = "Arthur"
reputation = 75

<< "Welcome, traveler!"

if reputation > 50
    << "Good to see you again, " + playerName + "!"
else
    << "I don't know you."
```

Output:
```
Welcome, traveler!
Good to see you again, Arthur!
```

## Key Features

- **Simple Syntax** - Easy to read and write, even for non-programmers
- **Indentation-based** - No curly braces or semicolons needed
- **Streaming Architecture** - Processes large scripts efficiently
- **Cross-platform** - Works with Unity, Godot, and any .NET environment
- **CLI Tool** - Test scripts directly from the command line

## Getting Started

Ready to start? Check out the [Installation Guide](./installation) to set up Game Dialog Script on your system.
