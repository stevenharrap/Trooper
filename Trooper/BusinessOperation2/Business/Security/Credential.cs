using Trooper.Interface.BusinessOperation2.Business.Security;

namespace Trooper.BusinessOperation2.Business.Security
{
    using System.Collections.Generic;

	public class Credential : ICredential
    {
        public string Username { get; set; }
                
        public IEnumerable<string> Groups { get; set; }
    }
}
