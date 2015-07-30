using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.DynamicServiceHost.Exceptions
{
    public class ComplexTypeConverterException : Exception
    {
        public ComplexTypeConverterException(string message)
            : base(message)
        {
        }
    }
}
