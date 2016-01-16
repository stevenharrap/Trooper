namespace Trooper.Thorny.Business.TestSuit.Deleting
{
    using Trooper.Interface.Thorny.Business.Operation.Single;

    public class DeletingRequirement<TPoco> : BaseRequirment<TPoco>
        where TPoco : class, new()
    {
        public IBusinessDelete<TPoco> Deleter { get; }        

        public DeletingRequirement(
            TestSuitHelper<TPoco> helper,
            IBusinessDelete<TPoco> deletet)
            : base(helper)
        {
            this.Deleter = deletet;
        }
    }
}
