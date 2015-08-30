namespace Trooper.Interface.Thorny.TestSuit
{
    using System.Collections;
using System.Collections.Generic;
using Trooper.Interface.Thorny.Business.Operation.Single;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;

    public interface ITestSuitHelper<TPoco>
        where TPoco : class
    {
        TPoco MakeValidItem();

        TPoco MakeInvalidItem();

        IIdentity MakeValidIdentity();

        IIdentity MakeInvalidIdentity();

        bool ItemExists(TPoco item, IBusinessRead<TPoco> boReader);

        void RemoveAllItems(IBusinessRead<TPoco> boReader, IBusinessDelete<TPoco> boDeleter);

        bool IdentifierAsEqual(TPoco itemA, TPoco itemB);

        bool NonIdentifersAsEqual(TPoco itemA, TPoco itemB);

        void CheckResponseForErrors(IResponse response);

        void ResponseFailsWithError(IResponse response, string code);

        void NoItemsExist(IBusinessRead<TPoco> boReader);
    }
}
