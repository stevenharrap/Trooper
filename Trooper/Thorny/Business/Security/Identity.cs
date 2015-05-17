using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Business.Security
{
    public class Identity : IIdentity
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public Guid Session { get; set; }

        public string Culture { get; set; }
    }
}
