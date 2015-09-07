using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Interface.Thorny.Business.Operation.Single;
using Trooper.Interface.Thorny.TestSuit;
using Trooper.Testing.CustomShopApi;
using Trooper.Testing.ShopModel.Poco;
using Trooper.Thorny.Business.TestSuit;
using Trooper.Thorny.Configuration;
using Autofac;
using Trooper.Interface.Thorny.Business.Operation.Composite;
using Trooper.Testing.CustomShopApi.Interface.Business.Operation;

namespace Trooper.Testing.CustomShopApiTestSuit.TestOutlet
{
    public class xx<TPoco> : x<TPoco>
        where TPoco : class
    {
        public IContainer Container { get; private set; }

        public xx(IContainer container, ITestSuitHelper<TPoco> helper, IBusinessCreate<TPoco> creater, IBusinessRead<TPoco> reader, IBusinessDelete<TPoco> deleter) :
            base(helper, creater, reader, deleter)
        {
            this.Container = container;
        }

        public override void Dispose()
        {
            BusinessModule.Stop(this.Container);            
            base.Dispose();
            this.Container = null;
        }
    }

    [TestFixture]
    public class TestAddingOutlet : Adding<Outlet>
    {
        public override Func<x<Outlet>> XMaker
        {
            get
            {
                return () =>
                {
                    var container = BusinessModule.Start<ShopAppModule>();
                    var helper = new TestAddingOutletHelper();
                    var reader = container.Resolve<IOutletBo>();
                    var creater = container.Resolve<IOutletBo>();
                    var deleter = container.Resolve<IOutletBo>();

                    return new xx<Outlet>(container, helper, creater, reader, deleter);
                };
            }
        }

        [SetUp]
        public void Setup()
        {
            //var container = BusinessModule.Start<ShopAppModule>();

            //this.Helper = new TestAddingOutletHelper();
            //this.Reader = container.Resolve<IOutletBo>();
            //this.Creater = container.Resolve<IOutletBo>();
            //this.Deleter = container.Resolve<IOutletBo>();
        }

        [TearDown]
        public void TearDown()
        {
            //this.Helper = null;
            //this.Reader = null;
            //this.Creater = null;
            //this.Deleter = null;
        }
        
    }
}
