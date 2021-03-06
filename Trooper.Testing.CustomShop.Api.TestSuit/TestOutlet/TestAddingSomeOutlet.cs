﻿namespace Trooper.Testing.CustomShop.Api.TestSuit
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

    [TestFixture]
    public class TestAddingSomeOutlet : TestAddingSome<Outlet>
    {
        private IContainer container;
        private AddingRequirement<Outlet> addingRequirement;

        public override Func<AddingRequirement<Outlet>> Requirement
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
            var helper = new TestOutletHelper(creater, creater, deleter);

            this.addingRequirement = new AddingRequirement<Outlet>(helper, creater);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            BusinessModule.Stop(this.container);
        }
    }
}
