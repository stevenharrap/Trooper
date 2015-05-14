namespace Trooper.Thorny.Business.Operation.Core
{
    using Trooper.Thorny.Interface.DataManager;

    public class Search : ISearch
    {
        public int SkipItems { get; set; }

        public int TakeItems { get; set; }
    }
}
