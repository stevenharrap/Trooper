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

                    return new x<Outlet>(helper, creater, reader, deleter);
                };
            }
        }

        [SetUp]
        public void Setup()
        {
            var container = BusinessModule.Start<ShopAppModule>();

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
