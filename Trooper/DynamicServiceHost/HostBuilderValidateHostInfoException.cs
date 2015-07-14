using System;

namespace Trooper.DynamicServiceHost
{
    public class HostBuilderValidateHostInfoException : Exception
    {
        public HostBuilderValidateHostInfoException(string message)
            : base(message)
        {
        }
    }
}
