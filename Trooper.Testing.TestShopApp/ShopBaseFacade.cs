using System.Collections.Generic;

namespace Trooper.Testing.DefaultShopApp
{
    using Autofac;
    using NUnit.Framework;
    using Trooper.BusinessOperation2.Injection;
    using Trooper.BusinessOperation2.Interface;
    using Trooper.BusinessOperation2.UnitTestBase;
    using Trooper.Testing.DefaultShopApi;
    using Trooper.Testing.DefaultShopApi.Interface.Business.Support;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Interface;

    [TestFixture]
    [Category("Facade")]
    public class TestShopBaseFacade : TestFacadeBase<IShopBusinessCore, Shop, IShop>
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
