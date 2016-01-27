namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using System.Linq;
    using Utility;
    using Response;
    using System;
    using Trooper.Interface.Thorny.Business.Operation.Core;

    public sealed class UpdatingDataStep<TEnt, TPoco> : IStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public void Execute(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));
            if (stepInfo.argument == null) throw new ArgumentNullException(nameof(stepInfo.argument));
            if (stepInfo.items == null || !stepInfo.items.Any()) throw new ArgumentException($"{nameof(stepInfo.items)} is null or empty");
            if (stepInfo.identity == null) throw new ArgumentNullException(nameof(stepInfo.identity));
            if (stepInfo.response == null) throw new ArgumentNullException(nameof(stepInfo.response));

            if (stepInfo.items.Count() > 1)
            {
                this.UpdateSome(stepInfo);
            }
            else
            {
                this.Update(stepInfo);
            }
        }        

        private void UpdateSome(IStepInfo<TEnt, TPoco> stepInfo)
        {            
            if (!(stepInfo.response is ManyResponse<TEnt>)) throw new ArgumentException($"{nameof(stepInfo.response)} is not a {nameof(ManyResponse<TEnt>)}");

            var manyResponse = stepInfo.response as ManyResponse<TEnt>;

            manyResponse.Items = stepInfo.items.Select(i =>
            {
                var item = stepInfo.businessPack.Facade.Update(i);

                if (item == null)
                {
                    var errorMessage = string.Format("The item ({0}) could not be updated.", typeof(TEnt));

                    MessageUtility.Errors.Add(errorMessage, BusinessCore.UpdateFailedCode, stepInfo.response);
                }

                return item;
            }).ToList();
        }

        private void Update(IStepInfo<TEnt, TPoco> stepInfo)
        {            
            if (!(stepInfo.response is SingleResponse<TEnt>)) throw new ArgumentException($"{nameof(stepInfo.response)} is not a {nameof(SingleResponse<TEnt>)}");

            var singleResponse = stepInfo.response as SingleResponse<TEnt>;
            singleResponse.Item = stepInfo.businessPack.Facade.Update(stepInfo.items.First());

            if (singleResponse.Item == null)
            {
                var errorMessage = string.Format("The item ({0}) could not be updated.", typeof(TEnt));

                MessageUtility.Errors.Add(errorMessage, BusinessCore.UpdateFailedCode, stepInfo.response);
            }
        }
    }
}
