namespace Trooper.Thorny.Business.TestSuit.Adding
{
    using Trooper.Interface.Thorny.Business.Operation.Single;

    public class AddingRequirement<TPoco> : BaseRequirment<TPoco>
        where TPoco : class, new()
    {
        public IBusinessCreate<TPoco> Creater { get; }        

        public AddingRequirement(
            TestSuitHelper<TPoco> helper,
            IBusinessCreate<TPoco> creater)
            : base(helper)
        {
            this.Creater = creater;
        }
    }
}
