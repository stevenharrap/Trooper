using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Business.Security
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
