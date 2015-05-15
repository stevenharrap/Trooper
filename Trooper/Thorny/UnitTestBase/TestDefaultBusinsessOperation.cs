namespace Trooper.Thorny.UnitTestBase
{
    using NUnit.Framework;
    using Trooper.Thorny.Injection;
    using Trooper.Thorny.UnitTestBase;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Interface.Thorny.Business.Security;

    public class TestDefaultBaseBusinsessOperation<Tc, Ti, TAppModule> : TestBusinessOperationBase<IBusinessCore<Tc, Ti>, Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
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
            var container = BusinessOperationInjection.BuildBusinessApp<TAppModule>();

            base.TestFixtureSetup(container);
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }        

        #endregion

        #region Tests

        #region DeleteAll

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Access_DeleteAll()
        {
            Assert.Inconclusive(accesInconclusive);
        }

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Validate_DeleteAll()
        {
            Assert.Inconclusive(validateInconclusive);
        }

        #endregion

        #region Add

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Access_Add()
        {
            Assert.Inconclusive(accesInconclusive);
        }

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Validate_Add()
        {
            Assert.Inconclusive(validateInconclusive);
        }

        #endregion

        #region AddSome

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Access_AddSome()
        {
            Assert.Inconclusive(accesInconclusive);
        }

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Validate_AddSome()
        {
            Assert.Inconclusive(validateInconclusive);
        }

        #endregion

        #region DeleteByKey

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Access_DeleteByKey()
        {
            Assert.Inconclusive(accesInconclusive);
        }

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Validate_DeleteByKey()
        {
            Assert.Inconclusive(validateInconclusive);
        }

        #endregion

        #region DeleteSomeByKey

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Access_DeleteSomeByKey()
        {
            Assert.Inconclusive(accesInconclusive);
        }

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Validate_DeleteSomeByKey()
        {
            Assert.Inconclusive(validateInconclusive);
        }

        #endregion

        #region GetAll

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Access_GetAll()
        {
            Assert.Inconclusive(accesInconclusive);
        }

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Validate_GetAll()
        {
            Assert.Inconclusive(validateInconclusive);
        }

        #endregion

        #region GetSome

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Access_GetSome()
        {
            Assert.Inconclusive(accesInconclusive);
        }

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Validate_GetSome()
        {
            Assert.Inconclusive(validateInconclusive);
        }

        #endregion

        #region GetByKey

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Access_GetByKey()
        {
            Assert.Inconclusive(accesInconclusive);
        }

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Validate_GetByKey()
        {
            Assert.Inconclusive(validateInconclusive);
        }

        #endregion

        #region ExistsByKey

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Access_ExistsByKey()
        {
            Assert.Inconclusive(accesInconclusive);
        }

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Validate_ExistsByKey()
        {
            Assert.Inconclusive(validateInconclusive);
        }

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

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Access_Update()
        {
            Assert.Inconclusive(accesInconclusive);            
        }

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Validate_Update()
        {
            Assert.Inconclusive(accesInconclusive);
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

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Access_Save()
        {
            Assert.Inconclusive(accesInconclusive);
        }

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Validate_Save()
        {
            Assert.Inconclusive(validateInconclusive);
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

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Access_SaveSome()
        {
            Assert.Inconclusive(accesInconclusive);
        }

        /// <summary>
        /// untestable here
        /// </summary>
        public override void Test_Validate_SaveSome()
        {
            Assert.Inconclusive(validateInconclusive);
        }

        #endregion

        #region Validate

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Base_Validate()
        {
            Assert.Inconclusive(validateInconclusive);
        }

        /// <summary>
        /// untestable here
        /// </summary>
        [Test]
        public override void Test_Access_Validate()
        {
            Assert.Inconclusive(accesInconclusive);
        }

        #endregion

        #endregion

        #region Support

        public override IIdentity GetInvalidIdentity()
        {
            throw new System.NotImplementedException();
        }

        public override Tc GetInvalidItem()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
