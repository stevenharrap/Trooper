namespace Trooper.Thorny.Business.Operation.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Trooper.Interface.Thorny.Business.Operation.Core;

    //todo: THIS IS NOT THREAD SAFE!!! NEEDS TO BE THREAD SAFE!!!
    public class SimpleCache<TEnt, TPoco> : ICache<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        private List<TPoco> cacheData = new List<TPoco>();

        public void Get(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo == null) throw new ArgumentNullException(nameof(stepInfo));
            if (stepInfo.items == null) throw new ArgumentNullException(nameof(stepInfo.items));
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));
            if (stepInfo.businessPack.Facade == null) throw new ArgumentNullException(nameof(stepInfo.businessPack.Facade));

            var facade = stepInfo.businessPack.Facade;

            stepInfo.cacheHits = from i in stepInfo.items
                                 let cacheItem = this.cacheData.FirstOrDefault(c => facade.AreEqual(c, i))
                                 let isHit = cacheItem != null
                                 select new CacheHit<TPoco>
                                 {
                                     Hit = isHit,
                                     Item = isHit ? facade.ToPoco(cacheItem) : null
                                 };
        }

        public void Put(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo == null) throw new ArgumentNullException(nameof(stepInfo));
            if (stepInfo.items == null) throw new ArgumentNullException(nameof(stepInfo.items));
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));
            if (stepInfo.businessPack.Facade == null) throw new ArgumentNullException(nameof(stepInfo.businessPack.Facade));

            var facade = stepInfo.businessPack.Facade;

            this.cacheData.RemoveAll(c => stepInfo.items.Any(i => facade.AreEqual(i, c)));
            this.cacheData.AddRange(facade.ToPocos(stepInfo.items));
        }

        public void Remove(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo == null) throw new ArgumentNullException(nameof(stepInfo));
            if (stepInfo.items == null) throw new ArgumentNullException(nameof(stepInfo.items));
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));
            if (stepInfo.businessPack.Facade == null) throw new ArgumentNullException(nameof(stepInfo.businessPack.Facade));

            var facade = stepInfo.businessPack.Facade;

            this.cacheData.RemoveAll(c => stepInfo.items.Any(i => facade.AreEqual(i, c)));
        }
    }
}
