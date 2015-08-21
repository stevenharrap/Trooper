namespace Trooper.Interface.Thorny.TestSuit
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Security.Principal;

    public interface ITestSuitHelper<TItem>
    {
        TItem GetValidItem();

        TItem GetInvalidItem();

        IIdentity GetValidIdentity();

        IIdentity GetInvalidIdentity();

        bool ItemExistsIn(TItem item, IEnumerable<TItem> items);

        bool ItemsAreEqual(TItem itemA, TItem itemB);
    }
}
