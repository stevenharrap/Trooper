namespace Trooper.Thorny.Business.Operation.Core.Step
{
    using System.Collections.Generic;
    using Trooper.Interface.Thorny.Business.Operation.Core;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;
    using System.Linq;
    using Utility;
    using System;
    using Interface.DataManager;
    using Response;

    public sealed class GetDataByKeyStep<TEnt, TPoco> : IBusinessProcessStep<TEnt, TPoco>
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
            if (!(response is ManyResponse<TEnt>)) throw new ArgumentException($"{nameof(response)} is not a {nameof(ManyResponse<TEnt>)}");

            var manyResponse = response as ManyResponse<TEnt>;

            manyResponse.Items = businessPack.Facade.GetSomeByKey(items).ToList();
        }

        public void Execute(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, TEnt item, IIdentity identity, IResponse response)
        {
            if (businessPack == null) throw new ArgumentNullException(nameof(businessPack));
            if (argument == null) throw new ArgumentNullException(nameof(argument));
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (identity == null) throw new ArgumentNullException(nameof(identity));
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (!(response is SingleResponse<TEnt>)) throw new ArgumentException($"{nameof(response)} is not a {nameof(SingleResponse<TEnt>)}");

            var singleResponse = response as SingleResponse<TEnt>;

            singleResponse.Item = businessPack.Facade.GetByKey(item);

            if (singleResponse.Item == null)
            {
                var errorMessage = string.Format("The ({0}) could not be found.", typeof(TEnt));
                MessageUtility.Errors.Add(errorMessage, BusinessCore.NoRecordCode, item, null, response);
            }
        }
    }
}
