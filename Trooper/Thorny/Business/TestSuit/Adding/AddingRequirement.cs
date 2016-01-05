namespace Trooper.Thorny.Business.TestSuit.Adding
{
    using Trooper.Interface.Thorny.Business.Operation.Single;

    public class AddingRequirment<TPoco> : BaseRequirment<TPoco>
        where TPoco : class, new()
    {
        public IBusinessCreate<TPoco> Creater { get; }        

        public AddingRequirment(
            TestSuitHelper<TPoco> helper,
            IBusinessCreate<TPoco> creater)
            : base(helper)
        {
            this.Creater = creater;
        }
    }
}
