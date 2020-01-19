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

namespace ConController {
    /// <summary>
    /// A class that contains multiple entry points
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class Controller : Attribute {
        /// <summary>
        /// Name of the controller for the first part of the CLI operand
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description for auto-generated documentation
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// A method that gets executed
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class EntryPoint : Attribute {
        /// <summary>
        /// Name of the entry point for the second part of the CLI operand
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description for auto-generated documentation
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// For setting parameter properties
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class Parameter : Attribute {
        /// <summary>
        /// This has to match the name of a parameter in your method
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description for auto-generated documentation
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// If false, then the parameter cannot be omitted. If true then you
        /// can specify a default in your code, like ", int repeat = 3) {"
        /// </summary>
        public bool Optional { get; set; } = true;
    }
}
