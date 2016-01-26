namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using System;

    public sealed class IsIdentityValidStep<TEnt, TPoco> : IStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public void Execute(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));
            if (stepInfo.argument == null) throw new ArgumentNullException(nameof(stepInfo.argument));
            if (stepInfo.response == null) throw new ArgumentNullException(nameof(stepInfo.response));

            stepInfo.businessPack.Authorization.IsValid(stepInfo.identity, stepInfo.response);
        }        
    }
}
