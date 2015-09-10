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
        where TPoco : class
    {
        public ITestSuitHelper<TPoco> Helper { get; private set; }

        public IBusinessCreate<TPoco> Creater { get; private set; }

        public IBusinessRead<TPoco> Reader { get; private set; }

        public IBusinessDelete<TPoco> Deleter { get; private set; }

        private IContainer container;

        public BaseRequirment(
            IContainer container,
            ITestSuitHelper<TPoco> helper,
            IBusinessCreate<TPoco> creater,
            IBusinessRead<TPoco> reader,
            IBusinessDelete<TPoco> deleter) : this(helper, creater, reader, deleter)
        {
            this.container = container;
        }

        public BaseRequirment(
            ITestSuitHelper<TPoco> helper, 
            IBusinessCreate<TPoco> creater, 
            IBusinessRead<TPoco> reader, 
            IBusinessDelete<TPoco> deleter)
        {
            this.Helper = helper;
            this.Creater = creater;
            this.Reader = reader;
            this.Deleter = deleter;
        }

        public void Dispose()
        {
            if (this.container != null)
            {
                BusinessModule.Stop(this.container);  
            }

            this.Helper = null;
            this.Creater = null;
            this.Reader = null;
            this.Deleter = null;
        }
    }
}
