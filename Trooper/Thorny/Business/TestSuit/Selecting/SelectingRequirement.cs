namespace Trooper.Thorny.Business.TestSuit.Selecting
{
    using Trooper.Interface.Thorny.Business.Operation.Single;

    public class SelectingRequirement<TPoco> : BaseRequirment<TPoco>
        where TPoco : class, new()
    {
        public IBusinessRead<TPoco> Reader { get; }        

        public SelectingRequirement(
            TestSuitHelper<TPoco> helper,
            IBusinessRead<TPoco> reader)
            : base(helper)
        {
            this.Reader = reader;
        }
    }
}
