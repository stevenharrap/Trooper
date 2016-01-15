namespace Trooper.Thorny.Business.TestSuit.Saving
{
    using Trooper.Interface.Thorny.Business.Operation.Single;

    public class SavingRequirement<TPoco> : BaseRequirment<TPoco>
        where TPoco : class, new()
    {
        public IBusinessSave<TPoco> Saver { get; }        

        public SavingRequirement(
            TestSuitHelper<TPoco> helper,
            IBusinessSave<TPoco> saver)
            : base(helper)
        {
            this.Saver = saver;
        }
    }
}
