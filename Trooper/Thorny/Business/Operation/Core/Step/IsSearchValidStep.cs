namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using System;
    using Utility;

    public sealed class IsSearchValidStep<TEnt, TPoco> : IStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public void Execute(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));
            if (stepInfo.search == null) throw new ArgumentNullException(nameof(stepInfo.search));

            if (stepInfo.search == null)
            {
                MessageUtility.Errors.Add("The search has not been supplied.", BusinessCore.InvalidSearchCode, stepInfo.response);
                return;
            }

            if (!stepInfo.businessPack.Facade.IsSearchAllowed(stepInfo.search))
            {
                MessageUtility.Errors.Add("The search type cannot be used for searching.", BusinessCore.DeniedSearchCode, stepInfo.response);
            }
        }

    }
}
