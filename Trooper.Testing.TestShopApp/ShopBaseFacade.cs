using System.Collections.Generic;

namespace Trooper.Testing.DefaultShopApp
{
    using Autofac;
    using NUnit.Framework;
    using Trooper.Thorny.Injection;
    using Trooper.Thorny.Interface;
    using Trooper.Thorny.UnitTestBase;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Testing.DefaultShopApi;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Interface;
    using Trooper.Testing.ShopModel.Model;

    [TestFixture]
    [Category("Facade")]
    public class TestShopBaseFacade : TestFacadeBase<IBusinessCore<Shop, IShop>, Shop, IShop>
    {
		private const string IgnoreInBase = "Ignored in base testing. Tested in specific facade.";

	    [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var container = BusinessOperationInjection.BuildBusinessApp<ShopAppModule>();

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
