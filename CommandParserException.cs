using System;

namespace ConController {
    /// <summary>
    /// User-friendly error message about errors in the CLI command
    /// </summary>
    public class CommandParserException : Exception {
        /// <summary>
        /// Constructor
        /// </summary>
        public CommandParserException() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public CommandParserException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        public CommandParserException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
