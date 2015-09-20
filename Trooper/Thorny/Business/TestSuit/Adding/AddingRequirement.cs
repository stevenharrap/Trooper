using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Interface.Thorny.Business.Operation.Single;
using Trooper.Interface.Thorny.TestSuit;
using Trooper.Thorny.Configuration;

namespace Trooper.Thorny.Business.TestSuit.Adding
{
    public class AddingRequirment<TPoco> : BaseRequirment<TPoco>
        where TPoco : class
    {
        public IBusinessCreate<TPoco> Creater { get; }        

        public AddingRequirment(
            ITestSuitHelper<TPoco> helper,
            IBusinessCreate<TPoco> creater)
            : base(helper)
        {
            this.Creater = creater;
        }
    }
}
