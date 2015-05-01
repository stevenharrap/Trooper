using System.Collections.Generic;

namespace Trooper.Interface.BusinessOperation2.Business.Security
{
	public interface ICredential : IIdentity
    {                    
        IEnumerable<string> Groups { get; set; }
    }
}
