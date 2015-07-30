using System;

namespace Trooper.DynamicServiceHost.Exceptions
{
    public class HostBuilderValidateHostInfoException : Exception
    {
        public HostBuilderValidateHostInfoException(string message)
            : base(message)
        {
        }
    }
}
