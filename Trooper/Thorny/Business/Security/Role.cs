using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Business.Security
{
    public class Role : List<IBehaviour>, IRole
	{
	}
}
