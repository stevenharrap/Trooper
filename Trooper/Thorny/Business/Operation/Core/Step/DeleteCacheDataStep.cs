namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using System;
    using Trooper.Interface.Thorny.Business.Operation.Core;

    public sealed class DeleteCacheDataStep<TEnt, TPoco> : IStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public void Execute(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo == null) throw new ArgumentNullException(nameof(stepInfo));
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));

            if (stepInfo.businessPack.Cache == null)
            {
                return;
            }

            stepInfo.businessPack.Cache.Remove(stepInfo);
        }
    }
}
