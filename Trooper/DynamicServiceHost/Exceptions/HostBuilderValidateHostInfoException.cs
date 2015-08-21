//--------------------------------------------------------------------------------------
// <copyright file="HostBuilderValidateHostInfoException.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.DynamicServiceHost.Exceptions
{
    using System;

    public class HostBuilderValidateHostInfoException : Exception
    {
        public HostBuilderValidateHostInfoException(string message)
            : base(message)
        {
        }
    }
}
