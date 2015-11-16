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
    public class BaseRequirment<TPoco> : IDisposable
        where TPoco : class, new()
    {
        public TestSuitHelper<TPoco> Helper { get; private set; }

        public delegate void Disposing(BaseRequirment<TPoco> br);

        public event Disposing OnDisposing;       

        public BaseRequirment(TestSuitHelper<TPoco> helper)
        {
            this.Helper = helper;
        }        

        public void Dispose()
        {
            if (this.OnDisposing != null)
            {
                this.OnDisposing(this);
            }
        }
    }
}
