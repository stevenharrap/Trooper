namespace Trooper.BusinessOperation2.Business.Security
{
    using System.Collections.Generic;
    using Trooper.BusinessOperation2.Interface.Business.Security;

    public class Credential : ICredential
    {
        public string Username { get; set; }
                
        public IEnumerable<string> Groups { get; set; }
    }
}
