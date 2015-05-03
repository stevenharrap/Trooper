namespace Trooper.NUnitTesting.TestShopApp.Actual
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
    public class TestShopActualBusinsessOperation : TestBusinessOperationBase<IShopBusinessCore, Shop, IShop>
    {
	    private const string ToBeImplemented = "Shop BO Test to be implemented";

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
		    Assert.IsTrue(false);
	    }

		[Test]
		[Ignore(ToBeImplemented)]
	    public override void TestUpdate()
	    {
			Assert.IsTrue(false);
	    }

		[Test]
		[Ignore(ToBeImplemented)]
	    public override void TestSave()
	    {
			Assert.IsTrue(false);
	    }

		[Test]
		[Ignore(ToBeImplemented)]
	    public override void TestSaveSome()
	    {
			Assert.IsTrue(false);
	    }

		[Test]
		[Ignore(ToBeImplemented)]
	    public override void TestValidate()
	    {
			Assert.IsTrue(false);
	    }
    }
}
