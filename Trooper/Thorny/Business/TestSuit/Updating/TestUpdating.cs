
namespace Trooper.Thorny.Business.TestSuit.Updating
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Operation.Core;
    using System.Collections.Generic;

    public abstract class TestUpdating<TPoco>
        where TPoco : class, new()
    {
        public abstract Func<UpdatingRequirement<TPoco>> Requirement { get; }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems();
                    var state = requirement.Helper.GetAllItems();

                    foreach (var item in validItems)
                    {
                        requirement.Helper.MakeInvalidItem(item);

                        var response = requirement.Updater.Update(item, allowedIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                    }
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems();
                    var state = requirement.Helper.GetAllItems();

                    foreach (var item in validItems)
                    {
                        requirement.Helper.MakeInvalidItem(item);

                        var response = requirement.Updater.Update(item, deniedIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                    }
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems();
                    var state = requirement.Helper.GetAllItems();

                    foreach (var item in validItems)
                    {
                        requirement.Helper.MakeInvalidItem(item);

                        var response = requirement.Updater.Update(item, invalidIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
                }

            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl))
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        requirement.Helper.AddValidItems();
                        var state = requirement.Helper.GetAllItems();

                        var response = requirement.Updater.Update(invalidItem, allowedIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode), Is.True);
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl))
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        requirement.Helper.AddValidItems();
                        var state = requirement.Helper.GetAllItems();

                        var response = requirement.Updater.Update(invalidItem, deniedIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl))
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        requirement.Helper.AddValidItems();
                        var state = requirement.Helper.GetAllItems();

                        var response = requirement.Updater.Update(invalidItem, invalidIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsAllowed_ReportsOkAndIsUpdatedAndOtherUnchanged()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var itemToUpdate in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var baseItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl, new List<TPoco> { itemToUpdate }).ToList();
                        var state = requirement.Helper.AddItems(baseItems);
                        requirement.Helper.CopyIdentifiers(state.Last(), itemToUpdate);

                        var response = requirement.Updater.Update(itemToUpdate, allowedIdentity);
                        var newState = requirement.Helper.GetAllItems();
                        var otherStateItems = state.Where(i => !requirement.Helper.IdentifiersAreEqual(i, response.Item));
                        var otherNewStateItems = newState.Where(i => !requirement.Helper.IdentifiersAreEqual(i, response.Item));

                        Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);
                        Assert.That(response.Item, Is.Not.Null);
                        Assert.That(requirement.Helper.AreEqual(response.Item, itemToUpdate));
                        Assert.That(requirement.Helper.AreEqual(otherStateItems, otherNewStateItems), Is.True);
                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.False);
                    }
            }
        }        

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var itemToUpdate in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var baseItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl, new List<TPoco> { itemToUpdate }).ToList();
                        var state = requirement.Helper.AddItems(baseItems);
                        requirement.Helper.CopyIdentifiers(state.Last(), itemToUpdate);

                        var response = requirement.Updater.Update(itemToUpdate, deniedIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var itemToUpdate in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var baseItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl, new List<TPoco> { itemToUpdate }).ToList();
                        var state = requirement.Helper.AddItems(baseItems);
                        requirement.Helper.CopyIdentifiers(state.Last(), itemToUpdate);

                        var response = requirement.Updater.Update(itemToUpdate, invalidIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var newValidItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();                        

                        var state = requirement.Helper.AddValidItems(newValidItem);

                        var response = requirement.Updater.Update(newValidItem, allowedIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode));
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var newValidItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var state = requirement.Helper.AddValidItems(newValidItem);
                        var response = requirement.Updater.Update(newValidItem, deniedIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                    }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var newValidItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var state = requirement.Helper.AddValidItems(newValidItem);

                        var response = requirement.Updater.Update(newValidItem, invalidIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl))
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Updater.Update(invalidItem, allowedIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems(false, TestSuitHelper.Keys.GenIfMnl))
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Updater.Update(invalidItem, deniedIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl))
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Updater.Update(invalidItem, invalidIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Updater.Update(validItem, allowedIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode));
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Updater.Update(validItem, deniedIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                    }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl))
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Updater.Update(validItem, invalidIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
            }
        }
    }
}