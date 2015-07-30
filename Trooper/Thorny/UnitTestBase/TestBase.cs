using Trooper.Interface.Thorny.Business.Operation.Core;

namespace Trooper.Thorny.UnitTestBase
{
	using Autofac;
	using Trooper.Thorny.Interface.UnitTestBase;

    public class TestBase<TiBusinessCore, TEnt, TPoco> : TestBase
        where TiBusinessCore : IBusinessCore<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        protected IItemGenerator<TEnt, TPoco> ItemGenerator { get; set; }

        protected IContainer Container { get; set; }

        public virtual void TestFixtureSetup(IContainer container, IItemGenerator<TEnt, TPoco> itemGenerator)
        {
            this.Container = container;
            this.ItemGenerator = itemGenerator;
        }

        public virtual void TestFixtureSetup(IContainer container)
        {
            this.TestFixtureSetup(container, new ItemGenerator<TEnt, TPoco>());
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
