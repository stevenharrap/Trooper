namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using System.Collections.Generic;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;
    using Utility;
    using Response;
    using System;
    using Interface.DataManager;

    public sealed class AddDataStep<TEnt, TPoco> : IBusinessProcessStep<TEnt, TPoco>
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
            if (businessPack == null) throw new ArgumentNullException(nameof(businessPack));
            if (argument == null) throw new ArgumentNullException(nameof(argument));
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (identity == null) throw new ArgumentNullException(nameof(identity));
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (!(response is AddSomeResponse<TEnt>)) throw new ArgumentException($"{nameof(response)} is not a {nameof(AddSomeResponse<TEnt>)}");

            var addResponse = response as AddSomeResponse<TEnt>;
            var added = businessPack.Facade.AddSome(items);

            if (added == null)
            {
                var errorMessage = string.Format("The entities ({0}) could not be added.", typeof(TEnt));

                MessageUtility.Errors.Add(errorMessage, BusinessCore.AddFailedCode, response);
            }

            addResponse.Items = added;
        }

        public void Execute(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, TEnt item, IIdentity identity, IResponse response)
        {
            if (businessPack == null) throw new ArgumentNullException(nameof(businessPack));
            if (argument == null) throw new ArgumentNullException(nameof(argument));
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (identity == null) throw new ArgumentNullException(nameof(identity));
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (!(response is AddResponse<TEnt>)) throw new ArgumentException($"{nameof(response)} is not a {nameof(AddResponse<TEnt>)}");

            var addResponse = response as AddResponse<TEnt>;
            var added = businessPack.Facade.Add(item);

            if (added == null)
            {
                var errorMessage = string.Format("The entity ({0}) could not be added.", typeof(TEnt));

                MessageUtility.Errors.Add(errorMessage, BusinessCore.AddFailedCode, response);
            }

            addResponse.Item = added;
        }
    }
}
