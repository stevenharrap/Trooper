namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using System.Linq;
    using Utility;
    using System;

    public sealed class DataExistsStep<TEnt, TPoco> : IStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public void Execute(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo == null) throw new ArgumentNullException(nameof(stepInfo));
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));
            if (stepInfo.items == null && stepInfo.item == null) throw new ArgumentNullException($"{nameof(stepInfo.items)} and {nameof(stepInfo.items)}");
            if (stepInfo.response == null) throw new ArgumentNullException(nameof(stepInfo.response));

            if ((stepInfo.item != null && stepInfo.businessPack.Facade.Exists(stepInfo.item)) 
                || (stepInfo.items != null && stepInfo.items.Any(item => stepInfo.businessPack.Facade.Exists(item))))
            {
                var errorMessage = string.Format("The item ({0}) already exists.", typeof(TEnt));

                MessageUtility.Errors.Add(errorMessage, BusinessCore.AddFailedCode, stepInfo.response);
            }
        }        
    }
}
