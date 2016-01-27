namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using System.Collections.Generic;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using System.Linq;
    using Utility;
    using Response;
    using System;

    public sealed class SaveDataStep<TEnt, TPoco> : IStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public void Execute(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (stepInfo.businessPack == null) throw new ArgumentNullException(nameof(stepInfo.businessPack));
            if (stepInfo.items == null || !stepInfo.items.Any()) throw new ArgumentException($"{nameof(stepInfo.items)} is null or empty");
            if (stepInfo.response == null) throw new ArgumentNullException(nameof(stepInfo.response));

            if (stepInfo.items.Count() > 1)
            {
                this.ExecuteSaveSome(stepInfo);
            }
            else
            {
                this.ExecuteSave(stepInfo);
            }
        }

        private void ExecuteSaveSome(IStepInfo<TEnt, TPoco> stepInfo)
        {
            //Todo: prevent multiple lookups to see that the item exists
            
            if (!(stepInfo.response is SaveSomeResponse<TEnt>)) throw new ArgumentException($"{nameof(stepInfo.response)} is not a {nameof(SaveSomeResponse<TEnt>)}");

            var saveResponse = stepInfo.response as SaveSomeResponse<TEnt>;
            var saved = new List<SaveSomeItem<TEnt>>();

            stepInfo.items.All(i =>
            {
                var exists = stepInfo.businessPack.Facade.Exists(i);
                var result = new SaveSomeItem<TEnt>
                {
                    Item = exists ? stepInfo.businessPack.Facade.Update(i) : stepInfo.businessPack.Facade.Add(i),
                    Change = exists ? SaveChangeType.Update : SaveChangeType.Add
                };

                if (result.Item == null)
                {
                    var errorMessage = string.Format("The ({0}) could not be saved.", typeof(TEnt));

                    MessageUtility.Errors.Add(errorMessage, BusinessCore.SaveFailedCode, stepInfo.response);
                    result.Change = SaveChangeType.None;
                }

                saved.Add(result);

                return stepInfo.response.Ok;
            });

            saveResponse.Items = saved;
        }

        private void ExecuteSave(IStepInfo<TEnt, TPoco> stepInfo)
        {
            if (!(stepInfo.response is SaveResponse<TEnt>)) throw new ArgumentException($"{nameof(stepInfo.response)} is not a {nameof(SaveResponse<TEnt>)}");

            var saveResponse = stepInfo.response as SaveResponse<TEnt>;
            var exists = stepInfo.businessPack.Facade.Exists(stepInfo.items.First());

            saveResponse.Item = exists 
                ? stepInfo.businessPack.Facade.Update(stepInfo.items.First()) 
                : stepInfo.businessPack.Facade.Add(stepInfo.items.First());

            if (saveResponse.Item == null)
            {
                var errorMessage = string.Format("The ({0}) could not be saved.", typeof(TEnt));

                MessageUtility.Errors.Add(errorMessage, BusinessCore.SaveFailedCode, stepInfo.response);
                saveResponse.Change = SaveChangeType.None;
            }
            else
            {
                saveResponse.Change = exists ? SaveChangeType.Update : SaveChangeType.Add;
            }
        }
    }
}
