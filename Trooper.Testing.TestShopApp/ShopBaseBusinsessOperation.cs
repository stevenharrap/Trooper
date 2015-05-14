namespace Trooper.Testing.DefaultShopApp
{
    using NUnit.Framework;
    using Trooper.Thorny.Injection;
    using Trooper.Thorny.UnitTestBase;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Testing.DefaultShopApi;
    using Trooper.Testing.ShopModel;
    using Trooper.Testing.ShopModel.Interface;
    using Trooper.Testing.ShopModel.Model;

    [TestFixture]
    [Category("BusinessOperation")]
    public class TestShopBaseBusinsessOperation : TestBusinessOperationBase<IBusinessCore<Shop, IShop>, Shop, IShop>
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

        public override void Test_Access_DeleteAll()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Validate_DeleteAll()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Access_Add()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Validate_Add()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Access_AddSome()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Validate_AddSome()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Access_DeleteByKey()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Validate_DeleteByKey()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Access_DeleteSomeByKey()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Validate_DeleteSomeByKey()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Access_GetAll()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Validate_GetAll()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Access_GetSome()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Validate_GetSome()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Access_GetByKey()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Validate_GetByKey()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Access_ExistsByKey()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Validate_ExistsByKey()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Base_IsAllowed()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Base_Update()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Access_Update()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Validate_Update()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Base_Save()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Access_Save()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Validate_Save()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Base_SaveSome()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Access_SaveSome()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Validate_SaveSome()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Base_Validate()
        {
            throw new System.NotImplementedException();
        }

        public override void Test_Access_Validate()
        {
            throw new System.NotImplementedException();
        }
    }
}
