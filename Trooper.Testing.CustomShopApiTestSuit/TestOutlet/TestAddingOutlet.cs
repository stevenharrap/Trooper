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
    [TestFixture]
    public class TestAddingOutlet : Adding<Outlet>
    {
        [SetUp]
        public void Setup()
        {
            var container = BusinessModule.Start<ShopAppModule>();

            this.Helper = new TestAddingOutletHelper();
            this.Reader = container.Resolve<IOutletBo>();
            this.Creater = container.Resolve<IOutletBo>();
            this.Deleter = container.Resolve<IOutletBo>();
        }

        [TearDown]
        public void TearDown()
        {
            this.Helper = null;
            this.Reader = null;
            this.Creater = null;
            this.Deleter = null;
        }
        
    }
}
