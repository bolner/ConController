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
