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
    using Thorny.Business.TestSuit.Self;

    [TestFixture]
    public class TestSelfOutlet : TestSelf<Outlet>
    {
        private IContainer container;
        private SelfRequirement<Outlet> selfRequirement;

        public override Func<SelfRequirement<Outlet>> Requirement
        {
            get
            {
                return () =>
                {
                    return this.selfRequirement;
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

            this.selfRequirement = new SelfRequirement<Outlet>(helper);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            BusinessModule.Stop(this.container);
        }
    }
}
