namespace Trooper.Thorny.Business.TestSuit.Deleting
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Operation.Core;
    using System.Collections.Generic;

    public abstract class TestDeletingSome<TPoco>
        where TPoco : class, new()
    {
        public abstract Func<DeletingRequirement<TPoco>> Requirement { get; }

        [Test]
        public virtual void HasItems_ItemsAllInvalidAllExist_IdentityIsAllowed_ReportsOkAndIsDeletedAndOtherUnchanged()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 2).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredValidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);

                    requirement.Helper.MakeInvalidItems(secondHalf, firstHalf);

                    var response = requirement.Deleter.DeleteSomeByKey(secondHalf, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(firstHalf));
                    Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllInvalidAllExist_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 2).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredValidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);
                    var state = requirement.Helper.GetAllItems();

                    requirement.Helper.MakeInvalidItems(secondHalf, firstHalf);

                    var response = requirement.Deleter.DeleteSomeByKey(secondHalf, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllInvalidAllExist_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 2).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredValidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);
                    var state = requirement.Helper.GetAllItems();

                    requirement.Helper.MakeInvalidItems(secondHalf, firstHalf);

                    var response = requirement.Deleter.DeleteSomeByKey(secondHalf, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllInvalidAllNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var state = requirement.Helper.AddValidItems();
                    var invalidItems = requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl, state);

                    var response = requirement.Deleter.DeleteSomeByKey(invalidItems, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllInvalidAllNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var state = requirement.Helper.AddValidItems();
                    var invalidItems = requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl, state);

                    var response = requirement.Deleter.DeleteSomeByKey(invalidItems, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllInvalidAllNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var state = requirement.Helper.AddValidItems();
                    var invalidItems = requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl, state);

                    var response = requirement.Deleter.DeleteSomeByKey(invalidItems, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllInvalidSomeExist_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 3).ToList();
                    var second = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);
                    requirement.Helper.MakeInvalidItems(second);
                    var third = requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.Gen, validItems);
                    var state = requirement.Helper.GetAllItems();
                    
                    var response = requirement.Deleter.DeleteSomeByKey(Enumerable.Concat(second, third), identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllInvalidSomeExist_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 3).ToList();
                    var second = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);
                    requirement.Helper.MakeInvalidItems(second);
                    var third = requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.Gen, validItems);
                    var state = requirement.Helper.GetAllItems();

                    var response = requirement.Deleter.DeleteSomeByKey(Enumerable.Concat(second, third), identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllInvalidSomeExist_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 3).ToList();
                    var second = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);
                    requirement.Helper.MakeInvalidItems(second);
                    var third = requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.Gen, validItems);
                    var state = requirement.Helper.GetAllItems();

                    var response = requirement.Deleter.DeleteSomeByKey(Enumerable.Concat(second, third), identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidAllExist_IdentityIsAllowed_ReportsOkAndIsDeletedAndOtherUnchanged()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 2).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredValidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);

                    var response = requirement.Deleter.DeleteSomeByKey(secondHalf, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(firstHalf));
                    Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidAllExist_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 2).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredValidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);
                    var state = requirement.Helper.GetAllItems();

                    var response = requirement.Deleter.DeleteSomeByKey(secondHalf, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidAllExist_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 2).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredValidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);
                    var state = requirement.Helper.GetAllItems();

                    var response = requirement.Deleter.DeleteSomeByKey(secondHalf, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidAllNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var state = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems).ToList();
                    var toDelete = requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.GenIfMnl, state);
                    var response = requirement.Deleter.DeleteSomeByKey(toDelete, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidAllNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var state = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems).ToList();
                    var toDelete = requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.GenIfMnl, state);
                    var response = requirement.Deleter.DeleteSomeByKey(toDelete, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidAllNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var state = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems).ToList();
                    var toDelete = requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.GenIfMnl, state);
                    var response = requirement.Deleter.DeleteSomeByKey(toDelete, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidSomeExist_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 3).ToList();
                    var second = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);
                    var third = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen, validItems);
                    var state = requirement.Helper.GetAllItems();
                    var toDelete = new List<TPoco>(second);
                    toDelete.AddRange(third);

                    var response = requirement.Deleter.DeleteSomeByKey(toDelete, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidSomeExist_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 3).ToList();
                    var second = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);
                    var third = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen, validItems);
                    var state = requirement.Helper.GetAllItems();
                    var toDelete = new List<TPoco>(second);
                    toDelete.AddRange(third);

                    var response = requirement.Deleter.DeleteSomeByKey(toDelete, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidSomeExist_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 3).ToList();
                    var second = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);
                    var third = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.Gen, validItems);
                    var state = requirement.Helper.GetAllItems();
                    var toDelete = new List<TPoco>(second);
                    toDelete.AddRange(third);

                    var response = requirement.Deleter.DeleteSomeByKey(toDelete, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsSomeValidAllExist_IdentityIsAllowed_ReportsOkAndIsDeletedAndOtherUnchanged()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 3).ToList();
                    var second = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);
                    var third = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems * 2, requirement.Helper.DefaultRequiredValidItems);
                    requirement.Helper.MakeInvalidItems(third);
                    var state = requirement.Helper.GetAllItems();
                    var toDelete = new List<TPoco>(second);
                    toDelete.AddRange(third);

                    var response = requirement.Deleter.DeleteSomeByKey(toDelete, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsSomeValidAllExist_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 3).ToList();
                    var second = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);
                    var third = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems * 2, requirement.Helper.DefaultRequiredValidItems);
                    requirement.Helper.MakeInvalidItems(third);
                    var state = requirement.Helper.GetAllItems();
                    var toDelete = new List<TPoco>(second);
                    toDelete.AddRange(third);

                    var response = requirement.Deleter.DeleteSomeByKey(toDelete, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsSomeValidAllExist_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems * 3).ToList();
                    var second = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems, requirement.Helper.DefaultRequiredValidItems);
                    var third = validItems.GetRange(requirement.Helper.DefaultRequiredValidItems * 2, requirement.Helper.DefaultRequiredValidItems);
                    requirement.Helper.MakeInvalidItems(third);
                    var state = requirement.Helper.GetAllItems();
                    var toDelete = new List<TPoco>(second);
                    toDelete.AddRange(third);

                    var response = requirement.Deleter.DeleteSomeByKey(toDelete, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsSomeValidAllNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var state = requirement.Helper.AddValidItems();
                    var second = requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.GenIfMnl, state);
                    var toDelete = new List<TPoco>(second);
                    var third = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl, Enumerable.Concat(state, second));
                    toDelete.AddRange(third);

                    var response = requirement.Deleter.DeleteSomeByKey(toDelete, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsSomeValidAllNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var state = requirement.Helper.AddValidItems();
                    var second = requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.GenIfMnl, state);
                    var toDelete = new List<TPoco>(second);
                    var third = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl, Enumerable.Concat(state, second));
                    toDelete.AddRange(third);

                    var response = requirement.Deleter.DeleteSomeByKey(toDelete, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsSomeValidAllNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var state = requirement.Helper.AddValidItems();
                    var second = requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.GenIfMnl, state);
                    var toDelete = new List<TPoco>(second);
                    var third = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl, Enumerable.Concat(state, second));
                    toDelete.AddRange(third);

                    var response = requirement.Deleter.DeleteSomeByKey(toDelete, identity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsAllInvalidAllNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();
                
                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                {
                    var invalidItems = requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl);
                    var response = requirement.Deleter.DeleteSomeByKey(invalidItems, identity);

                    Assert.That(requirement.Helper.HasNoItems());
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsAllInvalidAllNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                {
                    var invalidItems = requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl);
                    var response = requirement.Deleter.DeleteSomeByKey(invalidItems, identity);

                    Assert.That(requirement.Helper.HasNoItems());
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsAllInvalidAllNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                {
                    var invalidItems = requirement.Helper.MakeInvalidItems(true, TestSuitHelper.Keys.GenIfMnl);
                    var response = requirement.Deleter.DeleteSomeByKey(invalidItems, identity);

                    Assert.That(requirement.Helper.HasNoItems());
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsAllValidAllNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                {
                    var validItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl);
                    var response = requirement.Deleter.DeleteSomeByKey(validItems, identity);

                    Assert.That(requirement.Helper.HasNoItems());
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsAllValidAllNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                {
                    var validItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl);
                    var response = requirement.Deleter.DeleteSomeByKey(validItems, identity);

                    Assert.That(requirement.Helper.HasNoItems());
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsAllValidAllNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                {
                    var validItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl);
                    var response = requirement.Deleter.DeleteSomeByKey(validItems, identity);

                    Assert.That(requirement.Helper.HasNoItems());
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsSomeValidAllNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeAllowedIdentities())
                {
                    var validItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl);
                    var invalidItems = requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.GenIfMnl, validItems);

                    var response = requirement.Deleter.DeleteSomeByKey(Enumerable.Concat(validItems, invalidItems), identity);

                    Assert.That(requirement.Helper.HasNoItems());
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NoRecordCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsSomeValidAllNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeDeniedIdentities())
                {
                    var validItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl);
                    var invalidItems = requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.GenIfMnl, validItems);

                    var response = requirement.Deleter.DeleteSomeByKey(Enumerable.Concat(validItems, invalidItems), identity);

                    Assert.That(requirement.Helper.HasNoItems());
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode), Is.True);
                }
            }
        }

        [Test]
        public virtual void IsEmpty_ItemsSomeValidAllNew_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                foreach (var identity in requirement.Helper.MakeInvalidIdentities())
                {
                    var validItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl);
                    var invalidItems = requirement.Helper.MakeInvalidItems(TestSuitHelper.Keys.GenIfMnl, validItems);

                    var response = requirement.Deleter.DeleteSomeByKey(Enumerable.Concat(validItems, invalidItems), identity);

                    Assert.That(requirement.Helper.HasNoItems());
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode), Is.True);
                }
            }
        }
    }
}