namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using System.Collections.Generic;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;
    using Security;
    using Response;
    using System;
    using Interface.DataManager;

    public sealed class IsAllowedStep<TEnt, TPoco> : IBusinessProcessStep<TEnt, TPoco>
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
            if (!(response is SingleResponse<bool>)) throw new ArgumentException($"{nameof(response)} is not a {nameof(SingleResponse<bool>)}");

            var boolResponse = response as SingleResponse<bool>;
            var testArg = new RequestArg<TPoco> { Action = argument.Action };
            var testOutcome = businessPack.Authorization.IsAllowed(testArg, identity);

            boolResponse.Item = testOutcome;
        }

        public void Execute(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, TEnt item, IIdentity identity, IResponse response)
        {
            if (businessPack == null) throw new ArgumentNullException(nameof(businessPack));
            if (argument == null) throw new ArgumentNullException(nameof(argument));
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (identity == null) throw new ArgumentNullException(nameof(identity));
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (!(response is SingleResponse<bool>)) throw new ArgumentException($"{nameof(response)} is not a {nameof(SingleResponse<bool>)}");

            var boolResponse = response as SingleResponse<bool>;
            var testArg = new RequestArg<TPoco> { Action = argument.Action };
            var testOutcome = businessPack.Authorization.IsAllowed(testArg, identity);

            boolResponse.Item = testOutcome;
        }
    }
}
