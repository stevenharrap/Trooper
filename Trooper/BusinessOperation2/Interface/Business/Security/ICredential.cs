namespace Trooper.BusinessOperation2.Interface.Business.Security
{
    using System.Collections.Generic;

    public interface ICredential : IIdentity
    {                    
        IEnumerable<string> Groups { get; set; }
    }
}
