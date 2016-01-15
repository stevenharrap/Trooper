namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using System.Collections.Generic;
    using System.Linq;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;
    using System;
    using Interface.DataManager;
    using Response;

    public sealed class GetSomeStep<TEnt, TPoco> : IBusinessProcessStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public void Execute(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, IIdentity identity, IResponse response)
        {
            throw new NotImplementedException();
        }

        public void Execute(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, ISearch search, IIdentity identity, IResponse response)
        {
            if (businessPack == null) throw new ArgumentNullException(nameof(businessPack));
            if (search == null) throw new ArgumentNullException(nameof(search));
            if (!(response is ManyResponse<TEnt>)) throw new ArgumentException($"{nameof(response)} is not a {nameof(ManyResponse<TEnt>)}");

            var manyResponse = response as ManyResponse<TEnt>;
            var some = businessPack.Facade.GetSome(search);
            var limit = search.SkipItems > 0 && search.TakeItems > 0;

            if (limit)
            {
                manyResponse.Items = businessPack.Facade.Limit(some, search).ToList();
            }
            else
            {
                manyResponse.Items = some.ToList();
            }
        }

        public void Execute(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, IEnumerable<TEnt> items, IIdentity identity, IResponse response)
        {
            throw new NotImplementedException();
        }

        public void Execute(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, TEnt item, IIdentity identity, IResponse response)
        {
            throw new NotImplementedException();
        }
    }
}
