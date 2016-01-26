namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using System;

    public sealed class IsIdentityAllowedStep<TEnt, TPoco> : IStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public void Execute(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));
            if (stepInfo.argument == null) throw new ArgumentNullException(nameof(stepInfo.argument));
            if (stepInfo.identity == null) throw new ArgumentNullException(nameof(stepInfo.identity));
            if (stepInfo.response == null) throw new ArgumentNullException(nameof(stepInfo.response));

            stepInfo.businessPack.Authorization.IsAllowed(stepInfo.argument, stepInfo.identity, stepInfo.response);
        }        
    }
}
