---
sidebar_position: 3
---

# Syntax Guide

This guide covers the complete syntax of Game Dialog Script Language.

## Dialog Output

Use `<<` to output dialog lines:

```python
<< "Hello, world!"
<< "This is another line."
```

## Variables

Assign values to variables using `=`:

```python
name = "Arthur"
health = 100
isAlive = true
```

### Supported Types

- **Strings**: `"Hello"` or `'Hello'`
- **Numbers**: `42`, `3.14`, `-10`
- **Booleans**: `true`, `false`

## String Concatenation

Join strings with `+`:

```python
name = "Arthur"
<< "Hello, " + name + "!"
```

## Arithmetic Operations

Standard math operators are supported:

```python
x = 10 + 5    # Addition: 15
x = 10 - 5    # Subtraction: 5
x = 10 * 5    # Multiplication: 50
x = 10 / 5    # Division: 2
x = 10 % 3    # Modulo: 1
```

## Comparison Operators

```python
x == y    # Equal
x != y    # Not equal
x < y     # Less than
x > y     # Greater than
x <= y    # Less than or equal
x >= y    # Greater than or equal
```

## Boolean Logic

```python
a and b   # Logical AND
a or b    # Logical OR
not a     # Logical NOT
```

## Conditional Statements

### If Statement

```python
if health > 0
    << "You are alive!"
```

### If-Else Statement

```python
if reputation > 50
    << "Welcome, friend!"
else
    << "Who are you?"
```

### Nested Conditions

```python
if level >= 10
    if hasKey
        << "You may enter the dungeon."
    else
        << "You need a key."
else
    << "Come back when you're stronger."
```

## While Loops

Repeat actions while a condition is true:

```python
count = 3
while count > 0
    << "Countdown: " + count
    count = count - 1
<< "Blast off!"
```

## Indentation

Game Dialog Script uses indentation to define code blocks. You can use either spaces or tabs, but be consistent within a file.

```python
if condition
    # This is inside the if block
    << "Inside"
# This is outside the if block
<< "Outside"
```

## Comments

Comments start with `#`:

```python
# This is a comment
name = "Arthur"  # This is also a comment
```
