namespace Trooper.BusinessOperation2.Interface.Business.Security
{
    using System.Collections.Generic;

    public interface ICredential
    {
        string Username { get; set; }               

        IEnumerable<string> Groups { get; set; }
    }
}
