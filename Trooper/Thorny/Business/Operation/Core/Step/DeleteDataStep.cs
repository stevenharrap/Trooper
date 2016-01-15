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

    public sealed class DeleteDataStep<TEnt, TPoco> : IBusinessProcessStep<TEnt, TPoco>
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
            if (!(response is Response)) throw new ArgumentException($"{nameof(response)} is not a {nameof(Response)}");

            if (!businessPack.Facade.DeleteSome(items))
            {
                var errorMessage = string.Format("At least one of the entities ({0}) could not be deleted.", typeof(TEnt));
                MessageUtility.Errors.Add(errorMessage, BusinessCore.NoRecordCode, response);
            }
        }

        public void Execute(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, TEnt item, IIdentity identity, IResponse response)
        {
            if (businessPack == null) throw new ArgumentNullException(nameof(businessPack));
            if (argument == null) throw new ArgumentNullException(nameof(argument));
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (identity == null) throw new ArgumentNullException(nameof(identity));
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (!(response is Response)) throw new ArgumentException($"{nameof(response)} is not a {nameof(Response)}");

            if (!businessPack.Facade.Delete(item))
            {
                var errorMessage = string.Format("The entity ({0}) could not be deleted.", typeof(TEnt));
                MessageUtility.Errors.Add(errorMessage, BusinessCore.NoRecordCode, response);
            }
        }
    }
}
