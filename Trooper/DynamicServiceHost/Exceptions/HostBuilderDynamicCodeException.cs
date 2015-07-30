using System;
using System.Collections.Generic;

namespace Trooper.DynamicServiceHost.Exceptions
{
    public class HostBuilderDynamicCodeException : Exception
    {
        public string SourceCode { get; set; }

        public List<string> Errors { get; set; }

        public HostBuilderDynamicCodeException(string message, string sourceCode, List<string> errors) : base(message)
        {
            this.SourceCode = sourceCode;
            this.Errors = errors;
        }
    }
}
