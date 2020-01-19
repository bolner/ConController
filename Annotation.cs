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
