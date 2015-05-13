using Trooper.Interface.BusinessOperation2.Business.Security;

namespace Trooper.BusinessOperation2.Business.Security
{
    using System;
    using System.Collections.Generic;

	public class Credential : ICredential
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public Guid Session { get; set; }
                
        public IEnumerable<string> Groups { get; set; }
    }
}
