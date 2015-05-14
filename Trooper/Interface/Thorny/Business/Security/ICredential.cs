using System.Collections.Generic;

namespace Trooper.Interface.Thorny.Business.Security
{
	public interface ICredential : IIdentity
    {                    
        IEnumerable<string> Groups { get; set; }
    }
}
