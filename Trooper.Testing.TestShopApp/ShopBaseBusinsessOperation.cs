namespace Trooper.Testing.DefaultShopApp
{
    using NUnit.Framework;
    using Trooper.BusinessOperation2.Injection;
    using Trooper.BusinessOperation2.UnitTestBase;
    using Trooper.Testing.DefaultShopApi;
    using Trooper.Testing.DefaultShopApi.Interface.Business.Support;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Interface;

    [TestFixture]
    [Category("BusinessOperation")]
    public class TestShopBaseBusinsessOperation : TestBusinessOperationBase<IShopBusinessCore, Shop, IShop>
    {
        private const string ToBeImplemented = "Ignored in base testing.";

        #region Tests

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
        public override void TestIsAllowed()
        {
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
        public override void TestSave()
        {
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void TestSaveSome()
        {
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void TestValidate()
        {
        }

        #endregion
    }
}
