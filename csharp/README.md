# C# Solutions

The C# solution is intended to be developed, built, and ran using `dotnet core 3.1`.

## Tools

You may use any of the following with this C# project:

 * Visual Studio 2019 16.4 or later
 * Visual Studio Code with
    * C# extension
    * dotnet core 3.1 SDK installed
 * Any text editor
    * dotnet core 3.1 SDK installed


## Project structure:

There are two projects, `Kelson.Advent` contains all solutions, and `Kelson.Advent.Tests` contains all tests

Each days tests are in a file named `Day#Tests.cs`

The core solution code for each day is in the `Kelson.Advent` project under a folder named `Day#`

The input files for each day are in `Kelson.Advent/Day#/input.txt`

The "driver" code for each puzzle is in a static method in [Program.cs](Kelson.Advent/Program.cs)

## Commands

All commands are invoked from the `csharp` directory

### Build:
```
dotnet build
```

### Run Tests:
```
dotnet test
```

### Run todays puzzle:
```
dotnet run 
```

### Run specific days puzzles: 
Run only day 5
```
dotnet run days 5
```

To run days 1, 2, and 3
```
dotnet run days 1 2 3
```

### Run a day with detailed logging enabled:
```
dotnet run log days 5
```