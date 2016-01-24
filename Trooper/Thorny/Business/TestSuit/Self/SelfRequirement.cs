namespace Trooper.Thorny.Business.TestSuit.Self
{
    public class SelfRequirement<TPoco> : BaseRequirment<TPoco>
        where TPoco : class, new()
    { 
        public SelfRequirement(TestSuitHelper<TPoco> helper)
            : base(helper)
        {
        }
    }
}
