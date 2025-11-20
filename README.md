# Game Dialog Script CLI

A simple command-line tool for running Game Dialog Script Language files.

## What It Does

Game Dialog Script CLI is an interpreter that executes `.gds` (Game Dialog Script) files. It reads your dialog scripts and outputs the results directly to the console, making it easy to test and debug your game dialogs without running the full game.

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
