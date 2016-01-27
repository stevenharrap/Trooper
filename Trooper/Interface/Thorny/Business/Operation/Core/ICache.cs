namespace Trooper.Interface.Thorny.Business.Operation.Core
{
    using System.Collections.Generic;

    public interface ICache<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {        
        void Put(IStepInfo<TEnt, TPoco> stepInfo);

        void Get(IStepInfo<TEnt, TPoco> stepInfo);

        void Remove(IStepInfo<TEnt, TPoco> stepInfo);
    }
}