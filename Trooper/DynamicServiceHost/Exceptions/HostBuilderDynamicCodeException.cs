//--------------------------------------------------------------------------------------
// <copyright file="HostBuilderDynamicCodeException.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.DynamicServiceHost.Exceptions
{
    using System;
    using System.Collections.Generic;

    public class HostBuilderDynamicCodeException : Exception
    {
        public HostBuilderDynamicCodeException(string message, string sourceCode, List<string> errors) : base(message)
        {
            this.SourceCode = sourceCode;
            this.Errors = errors;
        }

        public string SourceCode { get; set; }

        public List<string> Errors { get; set; }
    }
}