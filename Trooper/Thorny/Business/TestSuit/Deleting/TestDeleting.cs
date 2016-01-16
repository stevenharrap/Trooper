namespace Trooper.Thorny.Business.TestSuit.Deleting
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Operation.Core;

    public abstract class TestDeleting<TPoco>
        where TPoco : class, new()
    {
        public abstract Func<DeletingRequirement<TPoco>> Requirement { get; }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsAllowed_ReportsOkAndIsDeletedAndOtherUnchanged()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                    for (var t = 0; t < requirement.Helper.DefaultRequiredInvalidItems; t++)
                    {
                        requirement.Helper.RemoveAllItems();

                        var validItems = requirement.Helper.AddValidItems();
                        var preState = requirement.Helper.GetAllItems();
                        var target = validItems[t];
                        requirement.Helper.MakeInvalidItem(target);
                        var response = requirement.Deleter.DeleteByKey(target, identity);
                        var postState = requirement.Helper.GetAllItems();
                        var expected = preState.Where(i => !requirement.Helper.IdentifiersAreEqual(i, target));

                        Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);
                        Assert.That(requirement.Helper.AreEqual(expected, postState), Is.True);
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeDeniedIdentities())                
                    for (var t = 0; t < requirement.Helper.DefaultRequiredInvalidItems; t++)
                    {
                        requirement.Helper.RemoveAllItems();

                        var validItems = requirement.Helper.AddValidItems();
                        var state = requirement.Helper.GetAllItems();
                        var target = validItems[t];
                        requirement.Helper.MakeInvalidItem(target);
                        var response = requirement.Deleter.DeleteByKey(target, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                    }                
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                    for (var t = 0; t < requirement.Helper.DefaultRequiredInvalidItems; t++)
                    {
                        requirement.Helper.RemoveAllItems();

                        var validItems = requirement.Helper.AddValidItems();
                        var state = requirement.Helper.GetAllItems();
                        var target = validItems[t];
                        requirement.Helper.MakeInvalidItem(target);
                        var response = requirement.Deleter.DeleteByKey(target, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }                
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems();
                    var invalidItems = requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.GenIfMnl, validItems);
                    var state = requirement.Helper.GetAllItems();

                    foreach (var invalidItem in invalidItems)
                    {
                        var response = requirement.Deleter.DeleteByKey(invalidItem, identity);
                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode));
                    }
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();
                    var validItems = requirement.Helper.AddValidItems();
                    var invalidItems = requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.GenIfMnl, validItems);
                    var state = requirement.Helper.GetAllItems();

                    foreach (var invalidItem in invalidItems)
                    {
                        var response = requirement.Deleter.DeleteByKey(invalidItem, identity);
                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                    }
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();
                    var validItems = requirement.Helper.AddValidItems();
                    var invalidItems = requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.GenIfMnl, validItems);
                    var state = requirement.Helper.GetAllItems();

                    foreach (var invalidItem in invalidItems)
                    {
                        var response = requirement.Deleter.DeleteByKey(invalidItem, identity);
                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsAllowed_ReportsOkAndIsDeletedAndOtherUnchanged()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                    for (var t = 0; t < requirement.Helper.DefaultRequiredValidItems; t++)
                    {
                        requirement.Helper.RemoveAllItems();

                        var validItems = requirement.Helper.AddValidItems();
                        var preState = requirement.Helper.GetAllItems();
                        var target = validItems[t];
                        var response = requirement.Deleter.DeleteByKey(target, identity);
                        var postState = requirement.Helper.GetAllItems();
                        var expected = preState.Where(i => !requirement.Helper.IdentifiersAreEqual(i, target));

                        Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);
                        Assert.That(requirement.Helper.AreEqual(expected, postState), Is.True);
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                    for (var t = 0; t < requirement.Helper.DefaultRequiredValidItems; t++)
                    {
                        requirement.Helper.RemoveAllItems();

                        var validItems = requirement.Helper.AddValidItems();
                        var preState = requirement.Helper.GetAllItems();
                        var target = validItems[t];
                        var response = requirement.Deleter.DeleteByKey(target, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(preState), Is.True);
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                    for (var t = 0; t < requirement.Helper.DefaultRequiredValidItems; t++)
                    {
                        requirement.Helper.RemoveAllItems();

                        var validItems = requirement.Helper.AddValidItems();
                        var preState = requirement.Helper.GetAllItems();
                        var target = validItems[t];
                        var response = requirement.Deleter.DeleteByKey(target, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(preState), Is.True);
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                    foreach (var newValidItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    {
                        requirement.Helper.RemoveAllItems();

                        var state = requirement.Helper.AddValidItems(newValidItem);
                        var response = requirement.Deleter.DeleteByKey(newValidItem, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode), Is.True);
                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                    foreach (var newValidItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    {
                        requirement.Helper.RemoveAllItems();

                        var state = requirement.Helper.AddValidItems(newValidItem);
                        var response = requirement.Deleter.DeleteByKey(newValidItem, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                    foreach (var newValidItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    {
                        requirement.Helper.RemoveAllItems();

                        var state = requirement.Helper.AddValidItems(newValidItem);
                        var response = requirement.Deleter.DeleteByKey(newValidItem, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidExists_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                    foreach (var existsInvalidItem in requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.Gen))
                    {
                        var response = requirement.Deleter.DeleteByKey(existsInvalidItem, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode), Is.True);
                        Assert.That(requirement.Helper.HasNoItems, Is.True);
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidExists_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                    foreach (var existsInvalidItem in requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.Gen))
                    {
                        var response = requirement.Deleter.DeleteByKey(existsInvalidItem, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                        Assert.That(requirement.Helper.HasNoItems, Is.True);
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidExists_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                    foreach (var existsInvalidItem in requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.Gen))
                    {
                        var response = requirement.Deleter.DeleteByKey(existsInvalidItem, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                        Assert.That(requirement.Helper.HasNoItems, Is.True);
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                    foreach (var newInvalidItem in requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.GenIfMnl))
                    {
                        var response = requirement.Deleter.DeleteByKey(newInvalidItem, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode), Is.True);
                        Assert.That(requirement.Helper.HasNoItems, Is.True);
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                    foreach (var newInvalidItem in requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.GenIfMnl))
                    {
                        var response = requirement.Deleter.DeleteByKey(newInvalidItem, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                        Assert.That(requirement.Helper.HasNoItems, Is.True);
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                    foreach (var newInvalidItem in requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.GenIfMnl))
                    {
                        var response = requirement.Deleter.DeleteByKey(newInvalidItem, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                        Assert.That(requirement.Helper.HasNoItems, Is.True);
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidExists_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                    foreach (var newValidItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen))
                    {
                        var response = requirement.Deleter.DeleteByKey(newValidItem, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode), Is.True);
                        Assert.That(requirement.Helper.HasNoItems, Is.True);
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidExists_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                    foreach (var newValidItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen))
                    {
                        var response = requirement.Deleter.DeleteByKey(newValidItem, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                        Assert.That(requirement.Helper.HasNoItems, Is.True);
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidExists_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                    foreach (var newValidItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen))
                    {
                        var response = requirement.Deleter.DeleteByKey(newValidItem, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                        Assert.That(requirement.Helper.HasNoItems, Is.True);
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                    foreach (var newValidItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    {
                        var response = requirement.Deleter.DeleteByKey(newValidItem, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode), Is.True);
                        Assert.That(requirement.Helper.HasNoItems, Is.True);
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                    foreach (var newValidItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    {
                        var response = requirement.Deleter.DeleteByKey(newValidItem, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                        Assert.That(requirement.Helper.HasNoItems, Is.True);
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                    foreach (var newValidItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    {
                        var response = requirement.Deleter.DeleteByKey(newValidItem, identity);

                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                        Assert.That(requirement.Helper.HasNoItems, Is.True);
                    }
            }
        }

        [Test]
        public void SelfTestHelper()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.SelfTestHelper();
            }
        }
    }
}