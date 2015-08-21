//--------------------------------------------------------------------------------------
// <copyright file="ComplexTypeConverterException.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.DynamicServiceHost.Exceptions
{
    using System;

    public class ComplexTypeConverterException : Exception
    {
        public ComplexTypeConverterException(string message)
            : base(message)
        {
        }
    }
}
