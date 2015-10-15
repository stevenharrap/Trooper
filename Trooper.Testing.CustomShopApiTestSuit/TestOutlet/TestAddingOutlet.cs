namespace Trooper.Testing.CustomShop.TestApi.TestOutlet
{
    using NUnit.Framework;
    using System;
    using CustomShopApi;
    using ShopPoco;
    using Thorny.Configuration;
    using Autofac;
    using Thorny.Business.TestSuit.Adding;
    using Api.Interface.Business.Operation;
    using TestSuit.Common;

    [TestFixture]
    public class TestAddingOutlet : Adding<Outlet>
    {
        private IContainer container;
        private AddingRequirment<Outlet> addingRequirement;

        public override Func<AddingRequirment<Outlet>> Requirement
        {
            get
            {
                return () =>
                {
                    return this.addingRequirement;
                };
            }
        }        

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var parameters = new BusinessModuleStartParameters { AutoStartServices = false };
            this.container = BusinessModule.Start<ShopAppModule>(parameters);

            var reader = container.Resolve<IOutletBo>();
            var creater = container.Resolve<IOutletBo>();
            var deleter = container.Resolve<IOutletBo>();
            var helper = new TestAddingOutletHelper(creater, creater, deleter);

            this.addingRequirement = new AddingRequirment<Outlet>(helper, creater);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            BusinessModule.Stop(this.container);
        }
    }
}
