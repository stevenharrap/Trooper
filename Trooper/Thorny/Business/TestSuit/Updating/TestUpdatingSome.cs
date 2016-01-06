namespace Trooper.Thorny.Business.TestSuit.Updating
{
    using NUnit.Framework;
    using System;
    using System.Linq;
    using Operation.Core;
    using System.Collections.Generic;

    public abstract class TestUpdatingSome<TPoco>
        where TPoco : class, new()
    {
        public abstract Func<UpdatingRequirement<TPoco>> Requirement { get; }

        [Test]
        public virtual void HasItems_ItemsAllInvalidAllExist_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var willBeInvalidItems = requirement.Helper.AddValidItems().ToList();
                    var state = requirement.Helper.GetAllItems();

                    requirement.Helper.MakeInvalidItems(willBeInvalidItems);

                    var response = requirement.Updater.UpdateSome(willBeInvalidItems, allowedIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllInvalidAllExist_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var willBeInvalidItems = requirement.Helper.AddValidItems().ToList();
                    var state = requirement.Helper.GetAllItems();

                    requirement.Helper.MakeInvalidItems(willBeInvalidItems);

                    var response = requirement.Updater.UpdateSome(willBeInvalidItems, deniedIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllInvalidAllExist_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var willBeInvalidItems = requirement.Helper.AddValidItems().ToList();
                    var state = requirement.Helper.GetAllItems();

                    requirement.Helper.MakeInvalidItems(willBeInvalidItems);

                    var response = requirement.Updater.UpdateSome(willBeInvalidItems, invalidIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllInvalidAllNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var invalidItems = requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl);
                    var state = requirement.Helper.GetAllItems();

                    var response = requirement.Updater.UpdateSome(invalidItems, allowedIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllInvalidAllNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var invalidItems = requirement.Helper.MakeInvalidItems(false, TestSuitHelper.Keys.GenIfMnl);
                    var state = requirement.Helper.GetAllItems();

                    var response = requirement.Updater.UpdateSome(invalidItems, deniedIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllInvalidAllNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var invalidItems = requirement.Helper.MakeInvalidItems(false, TestSuitHelper.Keys.GenIfMnl);
                    var state = requirement.Helper.GetAllItems();

                    var response = requirement.Updater.UpdateSome(invalidItems, invalidIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllInvalidSomeExist_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredItems * 2, TestSuitHelper.Keys.GenIfMnl).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredItems, requirement.Helper.DefaultRequiredItems);
                    firstHalf = requirement.Helper.AddItems(firstHalf).ToList();
                    requirement.Helper.MakeInvalidItems(secondHalf);
                    var state = requirement.Helper.GetAllItems();

                    var toUpdate = new List<TPoco>(firstHalf);
                    toUpdate.AddRange(secondHalf);

                    var response = requirement.Updater.UpdateSome(toUpdate, allowedIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllInvalidSomeExist_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredItems * 2, TestSuitHelper.Keys.GenIfMnl).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredItems, requirement.Helper.DefaultRequiredItems);
                    firstHalf = requirement.Helper.AddItems(firstHalf).ToList();
                    requirement.Helper.MakeInvalidItems(secondHalf);
                    var state = requirement.Helper.GetAllItems();

                    var toUpdate = new List<TPoco>(firstHalf);
                    toUpdate.AddRange(secondHalf);

                    var response = requirement.Updater.UpdateSome(toUpdate, deniedIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllInvalidSomeExist_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredItems * 2, TestSuitHelper.Keys.GenIfMnl).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredItems, requirement.Helper.DefaultRequiredItems);
                    firstHalf = requirement.Helper.AddItems(firstHalf).ToList();
                    requirement.Helper.MakeInvalidItems(secondHalf);
                    var state = requirement.Helper.GetAllItems();

                    var toUpdate = new List<TPoco>(firstHalf);
                    toUpdate.AddRange(secondHalf);

                    var response = requirement.Updater.UpdateSome(toUpdate, invalidIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidAllExist_IdentityIsAllowed_ReportsOkAndAllUpdatedAndOthersUnchanged()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 2).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredItems, requirement.Helper.DefaultRequiredItems);
                    requirement.Helper.ChangeNonIdentifiers(secondHalf, firstHalf);
                    var response = requirement.Updater.UpdateSome(secondHalf, allowedIdentity);
                    var postState = requirement.Helper.GetAllItems();

                    Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);
                    Assert.That(response.Items, Is.Not.Null);
                    Assert.That(requirement.Helper.AreEqual(secondHalf, response.Items), Is.True);
                    Assert.That(requirement.Helper.Contains(postState, firstHalf));
                    Assert.That(requirement.Helper.Contains(postState, response.Items));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidAllExist_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 2).ToList();
                    var state = requirement.Helper.GetAllItems();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredItems, requirement.Helper.DefaultRequiredItems);
                    requirement.Helper.ChangeNonIdentifiers(secondHalf, firstHalf);
                    var response = requirement.Updater.UpdateSome(secondHalf, deniedIdentity);

                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidAllExist_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 2).ToList();
                    var state = requirement.Helper.GetAllItems();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredItems, requirement.Helper.DefaultRequiredItems);
                    requirement.Helper.ChangeNonIdentifiers(secondHalf, firstHalf);
                    var response = requirement.Updater.UpdateSome(secondHalf, invalidIdentity);

                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidAllNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredInvalidItems * 2, TestSuitHelper.Keys.GenIfMnl).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredInvalidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredInvalidItems, requirement.Helper.DefaultRequiredInvalidItems);

                    requirement.Helper.AddItems(firstHalf);
                    var state = requirement.Helper.GetAllItems();
                    var response = requirement.Updater.UpdateSome(secondHalf, allowedIdentity);

                    Assert.That(requirement.Helper.ResponseIsOk(response), Is.False);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidAllNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredInvalidItems * 2, TestSuitHelper.Keys.GenIfMnl).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredInvalidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredInvalidItems, requirement.Helper.DefaultRequiredInvalidItems);

                    requirement.Helper.AddItems(firstHalf);
                    var state = requirement.Helper.GetAllItems();
                    var response = requirement.Updater.UpdateSome(secondHalf, deniedIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidAllNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredInvalidItems * 2, TestSuitHelper.Keys.GenIfMnl).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredInvalidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredInvalidItems, requirement.Helper.DefaultRequiredInvalidItems);

                    requirement.Helper.AddItems(firstHalf);
                    var state = requirement.Helper.GetAllItems();
                    var response = requirement.Updater.UpdateSome(secondHalf, invalidIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidSomeExist_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredInvalidItems * 2, TestSuitHelper.Keys.GenIfMnl).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredInvalidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredInvalidItems, requirement.Helper.DefaultRequiredInvalidItems);

                    firstHalf = requirement.Helper.AddItems(firstHalf).ToList();
                    requirement.Helper.ChangeNonIdentifiers(firstHalf);
                    var state = requirement.Helper.GetAllItems();
                    var toUpdate = new List<TPoco>(firstHalf);
                    toUpdate.AddRange(secondHalf);

                    var response = requirement.Updater.UpdateSome(toUpdate, allowedIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidSomeExist_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredInvalidItems * 2, TestSuitHelper.Keys.GenIfMnl).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredInvalidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredInvalidItems, requirement.Helper.DefaultRequiredInvalidItems);

                    firstHalf = requirement.Helper.AddItems(firstHalf).ToList();
                    requirement.Helper.ChangeNonIdentifiers(firstHalf);
                    var state = requirement.Helper.GetAllItems();
                    var toUpdate = new List<TPoco>(firstHalf);
                    toUpdate.AddRange(secondHalf);

                    var response = requirement.Updater.UpdateSome(toUpdate, deniedIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidSomeExist_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredInvalidItems * 2, TestSuitHelper.Keys.GenIfMnl).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredInvalidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredInvalidItems, requirement.Helper.DefaultRequiredInvalidItems);

                    firstHalf = requirement.Helper.AddItems(firstHalf).ToList();
                    requirement.Helper.ChangeNonIdentifiers(firstHalf);
                    var state = requirement.Helper.GetAllItems();
                    var toUpdate = new List<TPoco>(firstHalf);
                    toUpdate.AddRange(secondHalf);

                    var response = requirement.Updater.UpdateSome(toUpdate, invalidIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsSomeValidAllExist_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var items = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 2).ToList();
                    var state = requirement.Helper.GetAllItems();
                    var firstHalf = items.GetRange(0, requirement.Helper.DefaultRequiredValidItems);
                    var secondHalf = items.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);
                    requirement.Helper.MakeInvalidItems(firstHalf);
                    requirement.Helper.ChangeNonIdentifiers(secondHalf);
                    var toUpdate = new List<TPoco>(firstHalf);
                    toUpdate.AddRange(secondHalf);

                    var response = requirement.Updater.UpdateSome(toUpdate, allowedIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsSomeValidAllExist_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var items = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 2).ToList();
                    var state = requirement.Helper.GetAllItems();
                    var firstHalf = items.GetRange(0, requirement.Helper.DefaultRequiredValidItems);
                    var secondHalf = items.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);
                    requirement.Helper.MakeInvalidItems(firstHalf);
                    requirement.Helper.ChangeNonIdentifiers(secondHalf);
                    var toUpdate = new List<TPoco>(firstHalf);
                    toUpdate.AddRange(secondHalf);

                    var response = requirement.Updater.UpdateSome(toUpdate, deniedIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsSomeValidAllExist_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var items = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 2).ToList();
                    var state = requirement.Helper.GetAllItems();
                    var firstHalf = items.GetRange(0, requirement.Helper.DefaultRequiredValidItems);
                    var secondHalf = items.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);
                    requirement.Helper.MakeInvalidItems(firstHalf);
                    requirement.Helper.ChangeNonIdentifiers(secondHalf);
                    var toUpdate = new List<TPoco>(firstHalf);
                    toUpdate.AddRange(secondHalf);

                    var response = requirement.Updater.UpdateSome(toUpdate, invalidIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsSomeValidAllNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredInvalidItems * 2, TestSuitHelper.Keys.GenIfMnl).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredInvalidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredInvalidItems, requirement.Helper.DefaultRequiredInvalidItems);
                    var invalidItems = requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl);
                    requirement.Helper.AddItems(firstHalf);
                    var toUpdate = new List<TPoco>(secondHalf);
                    toUpdate.AddRange(invalidItems);
                    var state = requirement.Helper.GetAllItems();

                    var response = requirement.Updater.UpdateSome(toUpdate, allowedIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsSomeValidAllNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredInvalidItems * 2, TestSuitHelper.Keys.GenIfMnl).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredInvalidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredInvalidItems, requirement.Helper.DefaultRequiredInvalidItems);
                    var invalidItems = requirement.Helper.MakeInvalidItems(false, TestSuitHelper.Keys.GenIfMnl);
                    requirement.Helper.AddItems(firstHalf);
                    var toUpdate = new List<TPoco>(secondHalf);
                    toUpdate.AddRange(invalidItems);
                    var state = requirement.Helper.GetAllItems();

                    var response = requirement.Updater.UpdateSome(toUpdate, deniedIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsSomeValidAllNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredInvalidItems * 2, TestSuitHelper.Keys.GenIfMnl).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredInvalidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredInvalidItems, requirement.Helper.DefaultRequiredInvalidItems);
                    var invalidItems = requirement.Helper.MakeInvalidItems(false, TestSuitHelper.Keys.GenIfMnl);
                    requirement.Helper.AddItems(firstHalf);
                    var toUpdate = new List<TPoco>(secondHalf);
                    toUpdate.AddRange(invalidItems);
                    var state = requirement.Helper.GetAllItems();

                    var response = requirement.Updater.UpdateSome(toUpdate, invalidIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsAllInvalidAllNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var invalidItems = requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl);
                    var response = requirement.Updater.UpdateSome(invalidItems, allowedIdentity);

                    Assert.That(requirement.Helper.HasNoItems(), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsAllInvalidAllNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var invalidItems = requirement.Helper.MakeInvalidItems(false, TestSuitHelper.Keys.GenIfMnl);
                    var response = requirement.Updater.UpdateSome(invalidItems, deniedIdentity);

                    Assert.That(requirement.Helper.HasNoItems(), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsAllInvalidAllNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var invalidItems = requirement.Helper.MakeInvalidItems(false, TestSuitHelper.Keys.GenIfMnl);
                    var response = requirement.Updater.UpdateSome(invalidItems, invalidIdentity);

                    Assert.That(requirement.Helper.HasNoItems(), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsAllValidAllNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl);
                    var response = requirement.Updater.UpdateSome(validItems, allowedIdentity);
                    var stored = requirement.Helper.GetAllItems();

                    Assert.That(requirement.Helper.ResponseIsOk(response), Is.False);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.HasNoItems(), Is.True);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode));
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsAllValidAllNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeInvalidItems(false, TestSuitHelper.Keys.GenIfMnl);
                    var response = requirement.Updater.UpdateSome(validItems, deniedIdentity);

                    Assert.That(requirement.Helper.HasNoItems(), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsAllValidAllNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl);
                    var response = requirement.Updater.UpdateSome(validItems, invalidIdentity);

                    Assert.That(requirement.Helper.HasNoItems(), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsSomeValidAllNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl);
                    var invalidItems = requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl);
                    var toUpdate = new List<TPoco>(validItems);
                    toUpdate.AddRange(invalidItems);

                    var response = requirement.Updater.UpdateSome(toUpdate, allowedIdentity);

                    Assert.That(requirement.Helper.HasNoItems(), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsSomeValidAllNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl);
                    var invalidItems = requirement.Helper.MakeInvalidItems(false, TestSuitHelper.Keys.GenIfMnl);
                    var toUpdate = new List<TPoco>(validItems);
                    toUpdate.AddRange(invalidItems);

                    var response = requirement.Updater.UpdateSome(toUpdate, deniedIdentity);

                    Assert.That(requirement.Helper.HasNoItems(), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsSomeValidAllNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl);
                    var invalidItems = requirement.Helper.MakeInvalidItems(false, TestSuitHelper.Keys.GenIfMnl);
                    var toUpdate = new List<TPoco>(validItems);
                    toUpdate.AddRange(invalidItems);

                    var response = requirement.Updater.UpdateSome(toUpdate, invalidIdentity);

                    Assert.That(requirement.Helper.HasNoItems(), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                }
            }
        }        
    }
}
