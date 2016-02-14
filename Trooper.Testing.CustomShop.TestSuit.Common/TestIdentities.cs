using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Interface.Thorny.Business.Security;
using Trooper.Thorny.Business.Security;

namespace Trooper.Testing.CustomShop.TestSuit.Common
{
    public class TestIdentities
    {
        public static IIdentity AllowedIdentity1()
        {
            return new Identity
            {
                Username = "ValidTestUser",
                Password = "1234"
            };
        }

        public static IIdentity DeniedIdentity1()
        {
            return new Identity
            {
                Username = "InvalidTestUser",
                Password = "6543"
            };
        }
    }
}
