namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Utility;
    using Response;
    using System;

    public sealed class DeleteDataStep<TEnt, TPoco> : IStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public void Execute(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));
            if (stepInfo.items == null && stepInfo.item == null) throw new ArgumentNullException($"{nameof(stepInfo.items)} and {nameof(stepInfo.items)}");
            if (stepInfo.response == null) throw new ArgumentNullException(nameof(stepInfo.response));
            if (!(stepInfo.response is Response)) throw new ArgumentException($"{nameof(stepInfo.response)} is not a {nameof(Response)}");

            if (stepInfo.items != null)
            {
                if (!stepInfo.businessPack.Facade.DeleteSome(stepInfo.items))
                {
                    var errorMessage = string.Format("At least one of the entities ({0}) could not be deleted.", typeof(TEnt));
                    MessageUtility.Errors.Add(errorMessage, BusinessCore.NoRecordCode, stepInfo.response);
                }
            }
            else
            {
                if (!stepInfo.businessPack.Facade.Delete(stepInfo.item))
                {
                    var errorMessage = string.Format("The entity ({0}) could not be deleted.", typeof(TEnt));
                    MessageUtility.Errors.Add(errorMessage, BusinessCore.NoRecordCode, stepInfo.response);
                }
            }
        }        
    }
}
