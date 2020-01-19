using System;
using System.IO;
using System.Threading.Tasks;

namespace ConController.Test {
    [Controller(Name = "test", Description = "For development purposes")]
    public class TestController : ControllerBase {
        [EntryPoint(Name = "mult", Description = "Multiply two numbers")]
        [Parameter(Name = "left", Optional = false, Description = "First number")]
        [Parameter(Name = "right", Optional = false, Description = "Second number")]
        public static void Multiply(double left, double right) {
            Console.WriteLine($"\n {left} x {right} = {left * right}\n");
        }

        [EntryPoint(Name = "out", Description = "Output text")]
        [Parameter(Name = "text", Optional = false, Description = "Text to output")]
        [Parameter(Name = "repeat", Optional = true, Description = "How many times to repeat the text.")]
        public static async Task Output(string text, int repeat = 3) {
            for(int i = 0; i < repeat; i++) {
                await Console.Out.WriteLineAsync(text);
            }
        }
    }
}
