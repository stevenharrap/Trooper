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

        TPoco Copy(TPoco item);

        void Copy(TPoco source, TPoco destination);

        TPoco CopyIdentifiers(TPoco item);

        void CopyIdentifiers(TPoco source, TPoco destination);

        TPoco CopyNonIdentifiers(TPoco item);

        void ChangeNonIdentifiers(TPoco item);

        void CopyNonIdentifiers(TPoco source, TPoco destination);

        IIdentity MakeAllowedIdentity();

        IIdentity MakeDeniedIdentity();

        IIdentity MakeInvalidIdentity();

        IList<TPoco> AddItems(List<TPoco> validItems, IIdentity validIdentity);

        IList<TPoco> AddItems(List<TPoco> validItems);

        TPoco AddItem(TPoco validItem, IIdentity validIdentity);

        TPoco AddItem(TPoco validItem);

        TPoco AddItem(IIdentity validIdentity);

        TPoco AddItem();

        TPoco GetItem(TPoco exitingItem, IIdentity validIdentity);

        TPoco GetItem(TPoco existingItem);

        bool ItemExists(TPoco validItem, IIdentity validIdentity);

        bool ItemExists(TPoco validItem);

        void RemoveAllItems(IIdentity validIdentity);

        void RemoveAllItems();

        IList<TPoco> GetAllItems(IIdentity validIdentity);

        IList<TPoco> GetAllItems();

        bool ItemCountIs(int count, IIdentity validIdentity);

        bool ItemCountIs(int count);

        bool StoredItemsAreEqualTo(IList<TPoco> items);

        bool StoredItemsAreEqualTo(IList<TPoco> items, IIdentity validIdentity);

        bool AreEqual(TPoco itemA, TPoco itemB);

        bool IdentifiersAreEqual(TPoco itemA, TPoco itemB);

        bool NonIdentifersAreEqual(TPoco itemA, TPoco itemB);        

        bool ResponseIsOk(IResponse response);

        string ResponseNotOkMessages(IResponse response);

        bool ResponseFailsWithError(IResponse response, string code);

        bool NoItemsExist(IIdentity validIdentity);

        bool NoItemsExist();

        void SelfTestHelper();
    }
}
