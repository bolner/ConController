/*
    Copyright 2020 Tamas Bolner
    
    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at
    
      http://www.apache.org/licenses/LICENSE-2.0
    
    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;
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
