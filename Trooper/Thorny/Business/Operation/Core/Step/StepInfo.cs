namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;
    using Interface.DataManager;
    using System.Collections.Generic;

    public class StepInfo<TEnt, TPoco> : IStepInfo<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public IBusinessPack<TEnt, TPoco> businessPack { get; set; }

        public IRequestArg<TPoco> argument { get; set; }

        public IEnumerable<TEnt> items { get; set; }

        public IIdentity identity { get; set; }

        public IResponse response { get; set; }

        public ISearch search { get; set; }

        public IEnumerable<ICacheHit<TPoco>> cacheHits { get; set; }
    }
}
