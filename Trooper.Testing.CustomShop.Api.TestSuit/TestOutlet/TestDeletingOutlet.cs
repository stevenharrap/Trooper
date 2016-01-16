namespace Trooper.Testing.CustomShop.Api.TestSuit
{
    using NUnit.Framework;
    using System;
    using CustomShopApi;
    using ShopPoco;
    using Thorny.Configuration;
    using Autofac;
    using Thorny.Business.TestSuit.Adding;
    using Interface.Business.Operation;
    using CustomShop.TestSuit.Common;
    using Thorny.Business.TestSuit.Deleting;

    [TestFixture]
    public class TestDeletingOutlet : TestDeleting<Outlet>
    {
        private IContainer container;
        private DeletingRequirement<Outlet> deletingRequirement;

        public override Func<DeletingRequirement<Outlet>> Requirement
        {
            get
            {
                return () =>
                {
                    return this.deletingRequirement;
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

            this.deletingRequirement = new DeletingRequirement<Outlet>(helper, deleter);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            BusinessModule.Stop(this.container);
        }
    }
}
