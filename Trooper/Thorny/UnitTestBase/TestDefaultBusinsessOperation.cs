namespace Trooper.Thorny.UnitTestBase
{
    using NUnit.Framework;
    using Trooper.Thorny.Configuration;
    using Trooper.Thorny.UnitTestBase;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Interface.Thorny.Business.Security;

    public class TestDefaultBaseBusinsessOperation<TEnt, TPoco, TAppModule> : TestBusinessOperationBase<IBusinessCore<TEnt, TPoco>, TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
        where TAppModule : Autofac.Module, new()
    {
        private const string accesInconclusive = @"This method cannot be tested from this generic testing class. " +
            "You will need to override this test method from TestBusinessOperationBase. " + 
            "Test for a user who has access and a user does not have acccess.";

        private const string validateInconclusive = @"This method cannot be tested from this generic testing class. " +
            "You will need to override this test method from TestBusinessOperationBase. " +
            "Test against an item which has valid and invalid property values.";

        private const string baseInconclusive = @"This method cannot be tested from this generic testing class. " +
            "You will need to override this test method from TestBusinessOperationBase. " +
            "This should test the ability of the method to get or set data and avoid access and validation issues.";

        #region Setup

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var container = BusinessModule.Start<TAppModule>();
                //BusinessModuleBuilder.StartBusinessApp<TAppModule>();

            base.TestFixtureSetup(container);
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }        

        #endregion

        #region Tests

        #region Add

        #endregion

        #region AddSome

        #endregion

        #region DeleteByKey

        #endregion

        #region DeleteSomeByKey
        
        #endregion

        #region GetAll
        
        #endregion

        #region GetSome
    
        #endregion

        #region GetByKey
    
        #endregion

        #region GetSomeByKey

        #endregion

        #region ExistsByKey

        #endregion

        #region IsAllowed

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Base_IsAllowed()
        {
            Assert.Inconclusive(baseInconclusive);
        }

        #endregion

        #region Update

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Base_Update()
        {
            Assert.Inconclusive(baseInconclusive);
        }

        #endregion

        #region UpdateSome

        /// <summary>
		/// untestable here
		/// </summary>
		[Test]
		public override void Test_Base_UpdateSome()
		{
			Assert.Inconclusive(baseInconclusive);
		}
        
        #endregion

        #region Save

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Base_Save()
        {
            Assert.Inconclusive(baseInconclusive);
        }
        
        #endregion

        #region SaveSome

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Base_SaveSome()
        {
            Assert.Inconclusive(baseInconclusive);
        }
    
        #endregion

        #endregion

        #region Support

        public override IIdentity GetInvalidIdentity()
        {
            throw new System.NotImplementedException();
        }

        public override TEnt GetInvalidItem()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
