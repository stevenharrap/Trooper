namespace Trooper.Interface.Thorny.Business.Operation.Core
{
    using Response;
    using Security;
    using System.Collections.Generic;
    using Trooper.Thorny.Interface.DataManager;

    public interface IBusinessProcessStep<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        void Execute(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, TEnt item, IIdentity identity, IResponse response);

        void Execute(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, IEnumerable<TEnt> items, IIdentity identity, IResponse response);

        void Execute(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, IIdentity identity, IResponse response);

        void Execute(IBusinessPack<TEnt, TPoco> businessPack, IRequestArg<TPoco> argument, ISearch search, IIdentity identity, IResponse response);
    }
}
