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
    using Thorny.Business.TestSuit.Saving;

    [TestFixture]
    public class TestSavingOutlet : TestSaving<Outlet>
    {
        private IContainer container;
        private SavingRequirement<Outlet> updatingRequirement;

        public override Func<SavingRequirement<Outlet>> Requirement
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

            this.updatingRequirement = new SavingRequirement<Outlet>(helper, creater);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            BusinessModule.Stop(this.container);
        }
    }
}
