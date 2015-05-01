using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Interface.BusinessOperation2.Business.Security;

namespace Trooper.BusinessOperation2.Business.Security
{
    public class Identity : IIdentity
    {
        public string Username
        {
            get;
            set;
        }
    }
}
