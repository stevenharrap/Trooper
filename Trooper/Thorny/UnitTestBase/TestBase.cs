using Trooper.Interface.Thorny.Business.Operation.Core;

namespace Trooper.Thorny.UnitTestBase
{
	using Autofac;
	using Trooper.Thorny.Interface.UnitTestBase;

    public class TestBase<TiBusinessCore, Tc, Ti> : TestBase
        where TiBusinessCore : IBusinessCore<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        protected IItemGenerator<Tc, Ti> ItemGenerator { get; set; }

        protected IContainer Container { get; set; }

        public virtual void TestFixtureSetup(IContainer container, IItemGenerator<Tc, Ti> itemGenerator)
        {
            this.Container = container;
            this.ItemGenerator = itemGenerator;
        }

        public virtual void TestFixtureSetup(IContainer container)
        {
            this.TestFixtureSetup(container, new ItemGenerator<Tc, Ti>());
        }

        public virtual void SetUp() { }

        public TiBusinessCore NewBusinessCoreInstance()
        {
            return this.Container.Resolve<TiBusinessCore>();
        }        
    }

    public class TestBase
    {
        public const string ValidUsername = "ValidTestUser";

        public const string InvalidUsername = "InvalidTestUser";
    }
}
