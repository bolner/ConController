using System;

namespace ConController {
    class CommandParserException : Exception {
        public CommandParserException() : base() { }
        public CommandParserException(string message) : base(message) { }
        public CommandParserException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
