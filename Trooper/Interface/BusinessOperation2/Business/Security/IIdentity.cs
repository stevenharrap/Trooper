using System;

namespace Trooper.Interface.BusinessOperation2.Business.Security
{
    public interface IIdentity
    {
        string Username { get; set; }

        string Password { get; set; }

        Guid Session { get; set; }
    }
}
