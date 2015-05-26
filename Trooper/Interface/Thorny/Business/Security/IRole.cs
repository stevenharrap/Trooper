using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Interface.Thorny.Business.Security
{
    public interface IRole
    {
        IList<IBehaviour> Behaviours { get; set; }
    }
}
