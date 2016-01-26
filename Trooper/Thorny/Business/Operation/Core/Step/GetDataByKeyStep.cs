namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using System.Linq;
    using Utility;
    using System;
    using Response;

    public sealed class GetDataByKeyStep<TEnt, TPoco> : IStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public void Execute(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));
            if (stepInfo.items == null && stepInfo.item == null) throw new ArgumentNullException($"{nameof(stepInfo.items)} and {nameof(stepInfo.items)}");
            if (stepInfo.response == null) throw new ArgumentNullException(nameof(stepInfo.response));

            if (stepInfo.item != null)
            {
                this.ExecuteGetByKey(stepInfo);
            }
            else
            {
                this.ExecuteGetSomeByKey(stepInfo);
            }
        }

        private void ExecuteGetSomeByKey(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (!(stepInfo.response is ManyResponse<TEnt>)) throw new ArgumentException($"{nameof(stepInfo.response)} is not a {nameof(ManyResponse<TEnt>)}");

            var manyResponse = stepInfo.response as ManyResponse<TEnt>;

            manyResponse.Items = stepInfo.businessPack.Facade.GetSomeByKey(stepInfo.items).ToList();
        }

        private void ExecuteGetByKey(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (!(stepInfo.response is SingleResponse<TEnt>)) throw new ArgumentException($"{nameof(stepInfo.response)} is not a {nameof(SingleResponse<TEnt>)}");

            var singleResponse = stepInfo.response as SingleResponse<TEnt>;

            singleResponse.Item = stepInfo.businessPack.Facade.GetByKey(stepInfo.item);

            if (singleResponse.Item == null)
            {
                var errorMessage = string.Format("The ({0}) could not be found.", typeof(TEnt));
                MessageUtility.Errors.Add(errorMessage, BusinessCore.NoRecordCode, stepInfo.item, null, stepInfo.response);
            }
        }   
    }
}
