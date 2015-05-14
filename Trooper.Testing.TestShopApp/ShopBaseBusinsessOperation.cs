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
    using Trooper.Interface.Thorny.Business.Security;

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

        #endregion

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Access_DeleteAll()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Validate_DeleteAll()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Access_Add()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Validate_Add()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Access_AddSome()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Validate_AddSome()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Access_DeleteByKey()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Validate_DeleteByKey()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Access_DeleteSomeByKey()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Validate_DeleteSomeByKey()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Access_GetAll()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Validate_GetAll()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Access_GetSome()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Validate_GetSome()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Access_GetByKey()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Validate_GetByKey()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Access_ExistsByKey()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Validate_ExistsByKey()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Base_IsAllowed()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Base_Update()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Access_Update()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Validate_Update()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Base_Save()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Access_Save()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Validate_Save()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Base_SaveSome()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Access_SaveSome()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Validate_SaveSome()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Base_Validate()
        {
            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Access_Validate()
        {
            
        }

        public override IIdentity GetInvalidIdentity()
        {
            throw new System.NotImplementedException();
        }

        public override Shop GetInvalidItem()
        {
            throw new System.NotImplementedException();
        }
    }
}
