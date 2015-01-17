namespace Trooper.BusinessOperation2.Business.Operation.Core
{
    using Trooper.BusinessOperation2.Interface.DataManager;

    public class Search : ISearch
    {
        public int SkipItems { get; set; }

        public int TakeItems { get; set; }
    }
}
