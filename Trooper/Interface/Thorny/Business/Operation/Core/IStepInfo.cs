namespace Trooper.Interface.Thorny.Business.Operation.Core
{
    using Response;
    using Security;
    using System.Collections.Generic;
    using Trooper.Thorny.Interface.DataManager;

    public interface IStepInfo<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        IBusinessPack<TEnt, TPoco> businessPack { get; set; }

        IRequestArg<TPoco> argument { get; set; }

        TEnt item { get; set; }

        IEnumerable<TEnt> items { get; set; }

        IIdentity identity { get; set; }

        IResponse response { get; set; }

        ISearch search { get; set; }
    }
}
