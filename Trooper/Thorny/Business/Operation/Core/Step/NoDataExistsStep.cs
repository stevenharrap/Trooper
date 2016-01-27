namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using System.Linq;
    using Utility;
    using System;

    public sealed class NoDataExistsStep<TEnt, TPoco> : IStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public void Execute(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));
            if (stepInfo.items == null || !stepInfo.items.Any()) throw new ArgumentException($"{nameof(stepInfo.items)} is null or empty");
            if (stepInfo.response == null) throw new ArgumentNullException(nameof(stepInfo.response));

            if (stepInfo.items.Any(item => !stepInfo.businessPack.Facade.Exists(item)))
            {
                var errorMessage = string.Format("The item ({0}) does not exist.", typeof(TEnt));

                MessageUtility.Errors.Add(errorMessage, BusinessCore.NoRecordCode, stepInfo.response);
            }
        }        
    }
}