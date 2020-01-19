ConController
=============

Controller library for console applications. You can place the controllers in multiple files / classes, which is ideal for bigger projects. Parameters are converted automatically to the proper types.

Notes:
- As you'll see in the next examples, the first operand tells, which method to call:
  - [controller name]/[entry point name]
- Each method has to be static, but you can freely choose between sync and async modes of operation.
- All controller classes must extend the `ControllerBase`, because they are identified this way. (You don't have to implement any interface, no worries.)

# NuGet package

Available at: https://www.nuget.org/packages/ConController

To include it in a `.NET Core` project:

```shell
$ dotnet add package LinearTsvParser
```

# Example 1: sync with obligatory parameters

```csharp
[Controller(Name = "test", Description = "For development purposes")]
public class TestController : ControllerBase {
    [EntryPoint(Name = "mult", Description = "Multiply two numbers")]
    [Parameter(Name = "left", Optional = false, Description = "First number")]
    [Parameter(Name = "right", Optional = false, Description = "Second number")]
    public static void Multiply(double left, double right) {
        Console.WriteLine($"\n {left} x {right} = {left * right}\n");
    }
}
```

```shell
$ dotnet run test/mult left=34.5 right=2

34.5 x 2 = 69
```

You can use classic parameter syntax with double dash:

```shell
$ dotnet run test/mult --left=34.5 --right=2

34.5 x 2 = 69
```

# Example 2: async with optional parameter

```csharp
[Controller(Name = "test", Description = "For development purposes")]
public class TestController : ControllerBase {
    [EntryPoint(Name = "out", Description = "Output text")]
    [Parameter(Name = "text", Optional = false, Description = "Text to output")]
    [Parameter(Name = "repeat", Optional = true, Description = "How many times to repeat the text.")]
    public static async Task Output(string text, int repeat = 3) {
        for(int i = 0; i < repeat; i++) {
            await Console.Out.WriteLineAsync(text);
        }
    }
}
```

```shell
$ dotnet run test/out text="This is a text" fileName=output.txt

This is a text
This is a text
This is a text
```

You can use classic parameter syntax with double dash:

```shell
$ dotnet run test/out --text="This is a text" --fileName=output.txt

This is a text
This is a text
This is a text
```

# Example entry point: Program.cs

```csharp
using System;
using System.Threading;
using System.Globalization;
using System.Threading.Tasks;

namespace YourNameSpace {
    public class Program {
        public static async Task<int> Main(string[] args) {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            try {
                return await CLI.Run(args);
            } catch (CommandParserException ex) {
                Console.Error.WriteLine($"\nError: {ex.Message}\n");
                return 1;
            }
        }
    }
}
```

The error messages from `CommandParserException` are user-friendly. You can display them in themselves without the full stack.

# TODO

- Generate text documentation from the descriptions and argument types.
- Add more XML docs to the methods
