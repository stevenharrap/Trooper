using System;
using System.ServiceModel;

namespace Trooper.Interface.Thorny.Business.Security
{
    [ServiceContract]
    public interface IIdentity
    {
        string Username { get; set; }

        string Password { get; set; }

        Guid Session { get; set; }

        string Culture { get; set; }
    }
}
