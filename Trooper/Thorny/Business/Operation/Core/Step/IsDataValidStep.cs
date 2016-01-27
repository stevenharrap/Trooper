namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using System;

    public sealed class IsDataValidStep<TEnt, TPoco> : IStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public void Execute(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));
            if (stepInfo.response == null) throw new ArgumentNullException(nameof(stepInfo.response));

            if (stepInfo.items != null)
            {
                stepInfo.businessPack.Validation.Validate(stepInfo.items, stepInfo.response);
            }
            else
            {
                stepInfo.businessPack.Validation.Validate((TEnt)null, stepInfo.response);
            }
        }        
    }
}
