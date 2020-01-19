using System;
using System.Threading;
using System.Globalization;
using System.Threading.Tasks;

namespace ConController.Test {
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
