namespace Trooper.Thorny.Business.TestSuit.Updating
{
    using Trooper.Interface.Thorny.Business.Operation.Single;

    public class UpdatingRequirement<TPoco> : BaseRequirment<TPoco>
        where TPoco : class, new()
    {
        public IBusinessUpdate<TPoco> Updater { get; }        

        public UpdatingRequirement(
            TestSuitHelper<TPoco> helper,
            IBusinessUpdate<TPoco> updater)
            : base(helper)
        {
            this.Updater = updater;
        }
    }
}
