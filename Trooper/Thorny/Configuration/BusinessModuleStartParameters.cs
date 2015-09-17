using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Interface.Thorny.Configuration;

namespace Trooper.Thorny.Configuration
{
    public class BusinessModuleStartParameters : IBusinessModuleStartParameters
    {
        public bool AutoStartServices { get; set; } = true;
    }
}
