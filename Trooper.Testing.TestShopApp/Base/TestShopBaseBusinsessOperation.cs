namespace Trooper.NUnitTesting.TestShopApp.Base
{
    using NUnit.Framework;
    using Trooper.BusinessOperation2.Injection;
    using Trooper.BusinessOperation2.UnitTestBase;
    using Trooper.Testing.CoreShop;
    using Trooper.Testing.CoreShop.Interface.Business.Support;
    using Trooper.Testing.CoreShop.Interface.Model;
    using Trooper.Testing.CoreShop.Model;

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

        [Test]
        [Ignore(ToBeImplemented)]
        public override void TestIsAllowed()
        {
            Assert.That(true);
        }

        [Test]
        [Ignore(ToBeImplemented)]
        public override void TestUpdate()
        {
            Assert.That(true);
        }

        [Test]
        [Ignore(ToBeImplemented)]
        public override void TestSave()
        {
            Assert.That(true);
        }

        [Test]
        [Ignore(ToBeImplemented)]
        public override void TestSaveSome()
        {
            Assert.That(true);
        }

        [Test]
        [Ignore(ToBeImplemented)]
        public override void TestValidate()
        {
            Assert.That(true);
        }

        #endregion
    }
}
