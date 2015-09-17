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

        TPoco CopyItem(TPoco item);

        IIdentity MakeValidIdentity();

        IIdentity MakeInvalidIdentity();

        IList<TPoco> AddItems(List<TPoco> validItems, IIdentity validIdentity, IBusinessCreate<TPoco> boCreater, IBusinessRead<TPoco> boReader);

        IList<TPoco> AddItems(List<TPoco> validItems, IBusinessCreate<TPoco> boCreater, IBusinessRead<TPoco> boReader);

        TPoco AddItem(TPoco validItem, IIdentity validIdentity, IBusinessCreate<TPoco> boCreater, IBusinessRead<TPoco> boReader);

        TPoco AddItem(TPoco validItem, IBusinessCreate<TPoco> boCreater, IBusinessRead<TPoco> boReader);

        TPoco GetItem(TPoco exitingItem, IIdentity validIdentity, IBusinessRead<TPoco> boReader);

        TPoco GetItem(TPoco existingItem, IBusinessRead<TPoco> boReader);

        bool ItemExists(TPoco validItem, IIdentity validIdentity, IBusinessRead<TPoco> boReader);

        bool ItemExists(TPoco validItem, IBusinessRead<TPoco> boReader);

        void RemoveAllItems(IIdentity validIdentity, IBusinessRead<TPoco> boReader, IBusinessDelete<TPoco> boDeleter);

        void RemoveAllItems(IBusinessRead<TPoco> boReader, IBusinessDelete<TPoco> boDeleter);

        IList<TPoco> GetAllItems(IIdentity validIdentity, IBusinessRead<TPoco> boReader);

        IList<TPoco> GetAllItems(IBusinessRead<TPoco> boReader);

        bool ItemCountIs(int count, IIdentity validIdentity, IBusinessRead<TPoco> boReader);

        bool ItemCountIs(int count, IBusinessRead<TPoco> boReader);

        bool AreEqual(TPoco itemA, TPoco itemB);

        bool IdentifierAreEqual(TPoco itemA, TPoco itemB);

        bool NonIdentifersAreEqual(TPoco itemA, TPoco itemB);

        void ChangeNonIdentifiers(TPoco item);

        void ResponseIsOk(IResponse response);

        void ResponseFailsWithError(IResponse response, string code);

        void NoItemsExist(IIdentity validIdentity, IBusinessRead<TPoco> boReader);

        void NoItemsExist(IBusinessRead<TPoco> boReader);

        void SelfTestHelper();
    }
}
