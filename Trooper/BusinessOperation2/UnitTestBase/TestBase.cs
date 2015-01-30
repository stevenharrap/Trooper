using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.BusinessOperation2.Business.Security;
using Trooper.BusinessOperation2.Interface.Business.Operation.Core;
using Trooper.BusinessOperation2.Interface.Business.Security;
using Trooper.BusinessOperation2.Interface.UnitTestBase;

namespace Trooper.BusinessOperation2.UnitTestBase
{
    public class TestBase<TiBusinessCore, Tc, Ti>
        where TiBusinessCore : IBusinessCore<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        protected IItemGenerator<Tc, Ti> ItemGenerator { get; set; }

        protected IContainer Container { get; set; }

        public virtual void TestFixtureSetup(IContainer container, IItemGenerator<Tc, Ti> itemGenerator)
        {
            this.Container = container;
            this.ItemGenerator = itemGenerator;
        }

        public virtual void TestFixtureSetup(IContainer container)
        {
            this.TestFixtureSetup(container, new ItemGenerator<Tc, Ti>());
        }

        public virtual void SetUp() { }

        public TiBusinessCore NewBusinessCoreInstance()
        {
            return this.Container.Resolve<TiBusinessCore>();
        }        
    }
}
