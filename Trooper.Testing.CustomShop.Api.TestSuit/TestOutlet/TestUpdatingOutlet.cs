namespace Trooper.Testing.CustomShop.Api.TestSuit
{
    using NUnit.Framework;
    using System;
    using CustomShopApi;
    using ShopPoco;
    using Thorny.Configuration;
    using Autofac;
    using Interface.Business.Operation;
    using CustomShop.TestSuit.Common;
    using Thorny.Business.TestSuit.Updating;

    [TestFixture]
    public class TestUpdatingOutlet : TestUpdating<Outlet>
    {
        private IContainer container;
        private UpdatingRequirement<Outlet> updatingRequirement;

        public override Func<UpdatingRequirement<Outlet>> Requirement
        {
            get
            {
                return () =>
                {
                    return this.updatingRequirement;
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
            var helper = new TestOutletHelper(creater, creater, deleter);

            this.updatingRequirement = new UpdatingRequirement<Outlet>(helper, creater);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            BusinessModule.Stop(this.container);
        }
    }
}
