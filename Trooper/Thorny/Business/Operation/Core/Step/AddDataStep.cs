namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Utility;
    using Response;
    using System;
    using System.Linq;

    public sealed class AddDataStep<TEnt, TPoco> : IStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public void Execute(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo == null) throw new ArgumentNullException(nameof(stepInfo));
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));
            if (stepInfo.items == null || !stepInfo.items.Any()) throw new ArgumentException($"{nameof(stepInfo.items)} is null or empty");
            if (stepInfo.response == null) throw new ArgumentNullException(nameof(stepInfo.response));

            if (stepInfo.items.Count() > 1)
            {
                this.ExecuteAddSome(stepInfo);
            }
            else
            {
                this.ExecuteAdd(stepInfo);
            }
        }

        private void ExecuteAdd(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (!(stepInfo.response is AddResponse<TEnt>)) throw new ArgumentException($"{nameof(stepInfo.response)} is not a {nameof(AddResponse<TEnt>)}");

            var addResponse = stepInfo.response as AddResponse<TEnt>;
            var added = stepInfo.businessPack.Facade.Add(stepInfo.items.First());

            if (added == null)
            {
                var errorMessage = string.Format("The entity ({0}) could not be added.", typeof(TEnt));

                MessageUtility.Errors.Add(errorMessage, BusinessCore.AddFailedCode, stepInfo.response);
            }

            addResponse.Item = added;
        }

        private void ExecuteAddSome(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (!(stepInfo.response is AddSomeResponse<TEnt>)) throw new ArgumentException($"{nameof(stepInfo.response)} is not a {nameof(AddSomeResponse<TEnt>)}");

            var addResponse = stepInfo.response as AddSomeResponse<TEnt>;
            var added = stepInfo.businessPack.Facade.AddSome(stepInfo.items);

            if (added == null)
            {
                var errorMessage = string.Format("The entities ({0}) could not be added.", typeof(TEnt));

                MessageUtility.Errors.Add(errorMessage, BusinessCore.AddFailedCode, stepInfo.response);
            }

            addResponse.Items = added;
        }        
    }
}
