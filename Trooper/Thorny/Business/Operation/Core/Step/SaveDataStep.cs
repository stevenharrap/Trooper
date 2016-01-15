namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using System.Collections.Generic;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;
    using System.Linq;
    using Utility;
    using Response;
    using System;
    using Interface.DataManager;

    public sealed class SaveDataStep<TEnt, TPoco> : IBusinessProcessStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public void Execute(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, IIdentity identity, IResponse response)
        {
            throw new NotImplementedException();
        }

        public void Execute(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, ISearch search, IIdentity identity, IResponse response)
        {
            throw new NotImplementedException();
        }

        public void Execute(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, IEnumerable<TEnt> items, IIdentity identity, IResponse response)
        {
            //Todo: prevent multiple lookups to see that the item exists

            if (businessPack == null) throw new ArgumentNullException(nameof(businessPack));
            if (argument == null) throw new ArgumentNullException(nameof(argument));
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (identity == null) throw new ArgumentNullException(nameof(identity));
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (!(response is SaveSomeResponse<TEnt>)) throw new ArgumentException($"{nameof(response)} is not a {nameof(SaveSomeResponse<TEnt>)}");

            var saveResponse = response as SaveSomeResponse<TEnt>;
            var saved = new List<SaveSomeItem<TEnt>>();

            items.All(i =>
            {
                var exists = businessPack.Facade.Exists(i);
                var result = new SaveSomeItem<TEnt>
                {
                    Item = exists ? businessPack.Facade.Update(i) : businessPack.Facade.Add(i),
                    Change = exists ? SaveChangeType.Update : SaveChangeType.Add
                };

                if (result.Item == null)
                {
                    var errorMessage = string.Format("The ({0}) could not be saved.", typeof(TEnt));

                    MessageUtility.Errors.Add(errorMessage, BusinessCore.SaveFailedCode, response);
                    result.Change = SaveChangeType.None;
                }

                saved.Add(result);

                return response.Ok;
            });

            saveResponse.Items = saved;
        }

        public void Execute(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, TEnt item, IIdentity identity, IResponse response)
        {
            if (businessPack == null) throw new ArgumentNullException(nameof(businessPack));
            if (argument == null) throw new ArgumentNullException(nameof(argument));
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (identity == null) throw new ArgumentNullException(nameof(identity));
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (!(response is SaveResponse<TEnt>)) throw new ArgumentException($"{nameof(response)} is not a {nameof(SaveResponse<TEnt>)}");

            var saveResponse = response as SaveResponse<TEnt>;
            var exists = businessPack.Facade.Exists(item);

            saveResponse.Item = exists ? businessPack.Facade.Update(item) : businessPack.Facade.Add(item);

            if (saveResponse.Item == null)
            {
                var errorMessage = string.Format("The ({0}) could not be saved.", typeof(TEnt));

                MessageUtility.Errors.Add(errorMessage, BusinessCore.SaveFailedCode, response);
                saveResponse.Change = SaveChangeType.None;
            }
            else
            {
                saveResponse.Change = exists ? SaveChangeType.Update : SaveChangeType.Add;
            }
        }
    }
}
