namespace Trooper.Interface.Thorny.TestSuit
{
    using System.Collections.Generic;
    using Business.Response;
    using Business.Security;

    public interface ITestSuitHelper<TPoco>
        where TPoco : class
    {
        IEnumerable<TPoco> MakeValidItems();

        IEnumerable<TPoco> MakeInvalidItems();

        TPoco Copy(TPoco item);

        void Copy(TPoco source, TPoco destination);
        
        TPoco CopyIdentifiers(TPoco item);

        void CopyIdentifiers(TPoco source, TPoco destination);

        TPoco CopyNonIdentifiers(TPoco item);

        void ChangeNonIdentifiers(TPoco item);

        void CopyNonIdentifiers(TPoco source, TPoco destination);

        IEnumerable<IIdentity> MakeAllowedIdentities();

        IEnumerable<IIdentity> MakeDeniedIdentities();

        IEnumerable<IIdentity> MakeInvalidIdentities();

        IIdentity GetAdminIdentity();

        IList<TPoco> AddItems(List<TPoco> validItems);

        IList<TPoco> AddValidItems();

        TPoco AddItem(TPoco validItem);

        TPoco GetItem(TPoco exitingItem);

        bool ItemExists(TPoco validItem);

        void RemoveAllItems();

        IList<TPoco> GetAllItems();

        bool ItemCountIs(int count);

        bool StoredItemsAreEqualTo(IList<TPoco> items);

        bool AreEqual(TPoco itemA, TPoco itemB);

        bool IdentifiersAreEqual(TPoco itemA, TPoco itemB);

        bool NonIdentifersAreEqual(TPoco itemA, TPoco itemB);        

        bool ResponseIsOk(IResponse response);

        string ResponseNotOkMessages(IResponse response);

        bool ResponseFailsWithError(IResponse response, string code);

        bool NoItemsExist();

        void SelfTestHelper();
    }
}
