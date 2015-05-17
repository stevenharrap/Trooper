using System;

namespace Trooper.Interface.Thorny.Business.Security
{
    public interface IIdentity
    {
        string Username { get; set; }

        string Password { get; set; }

        Guid Session { get; set; }

        string Culture { get; set; }
    }
}
