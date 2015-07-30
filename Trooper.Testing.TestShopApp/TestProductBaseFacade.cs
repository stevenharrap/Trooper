using System.Collections.Generic;

namespace Trooper.Testing.DefaultShopApp
{
    using Autofac;
    using NUnit.Framework;
    using Trooper.Thorny.Configuration;
    using Trooper.Thorny.Interface;
    using Trooper.Thorny.UnitTestBase;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Testing.DefaultShopApi;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Poco;
    using Trooper.Testing.ShopModel.Model;

    [TestFixture]
    [Category("Facade")]
    public class TestProductBaseFacade : TestFacadeBase<IBusinessCore<ProductEnt, Product>, ProductEnt, Product>
    {
		private const string IgnoreInBase = "Ignored in base testing. Tested in specific facade.";

	    [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var container = BusinessModule.Start<ShopAppModule>();
                
                //BusinessModuleBuilder.StartBusinessApp<ShopAppModule>();

            base.TestFixtureSetup(container);
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void TestUpdate()
        {
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void TestGetSome()
        {
        }
    }
}
