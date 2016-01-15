namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using System.Collections.Generic;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;
    using Response;
    using System;
    using Interface.DataManager;

    public sealed class GetSessionStep<TEnt, TPoco> : IBusinessProcessStep<TEnt, TPoco>
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
            if (identity == null) throw new ArgumentNullException(nameof(identity));
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (!(response is SingleResponse<Guid>)) throw new ArgumentException($"{nameof(response)} is not a {nameof(SingleResponse<Guid>)}");

            //Todo: ??????
        }

        public void Execute(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, TEnt item, IIdentity identity, IResponse response)
        {
            if (businessPack == null) throw new ArgumentNullException(nameof(businessPack));
            if (argument == null) throw new ArgumentNullException(nameof(argument));
            if (identity == null) throw new ArgumentNullException(nameof(identity));
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (!(response is SingleResponse<Guid>)) throw new ArgumentException($"{nameof(response)} is not a {nameof(SingleResponse<Guid>)}");

            //Todo: ??????
        }
    }
}
