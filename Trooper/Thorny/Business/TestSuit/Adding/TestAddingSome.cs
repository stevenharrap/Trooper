namespace Trooper.Thorny.Business.TestSuit.Adding
{
    using NUnit.Framework;
    using System;
    using System.Linq;
    using Operation.Core;
    using System.Collections.Generic;

    public abstract class TestAddingSome<TPoco>
        where TPoco : class, new()
    {
        public abstract Func<AddingRequirement<TPoco>> Requirement { get; }

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

                    var response = requirement.Creater.AddSome(willBeInvalidItems, allowedIdentity);

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

                    var response = requirement.Creater.AddSome(willBeInvalidItems, deniedIdentity);

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

                    var response = requirement.Creater.AddSome(willBeInvalidItems, invalidIdentity);

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

                    var response = requirement.Creater.AddSome(invalidItems, allowedIdentity);

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

                    var response = requirement.Creater.AddSome(invalidItems, deniedIdentity);

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

                    var response = requirement.Creater.AddSome(invalidItems, invalidIdentity);

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

                    var toAdd = new List<TPoco>(firstHalf);
                    toAdd.AddRange(secondHalf); 

                    var response = requirement.Creater.AddSome(toAdd, allowedIdentity);

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

                    var toAdd = new List<TPoco>(firstHalf);
                    toAdd.AddRange(secondHalf);

                    var response = requirement.Creater.AddSome(toAdd, deniedIdentity);

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

                    var toAdd = new List<TPoco>(firstHalf);
                    toAdd.AddRange(secondHalf);

                    var response = requirement.Creater.AddSome(toAdd, invalidIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidAllExist_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.AddValidItems();
                    var state = requirement.Helper.GetAllItems();
                    requirement.Helper.ChangeNonIdentifiers(validItems);
                    var response = requirement.Creater.AddSome(validItems, allowedIdentity);

                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.AddFailedCode));
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

                    var validItems = requirement.Helper.AddValidItems();
                    var state = requirement.Helper.GetAllItems();
                    requirement.Helper.ChangeNonIdentifiers(validItems);
                    var response = requirement.Creater.AddSome(validItems, deniedIdentity);

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

                    var validItems = requirement.Helper.AddValidItems();
                    var state = requirement.Helper.GetAllItems();
                    requirement.Helper.ChangeNonIdentifiers(validItems);
                    var response = requirement.Creater.AddSome(validItems, invalidIdentity);

                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidAllNew_IdentityIsAllowed_ReportsOkAndAllAddedAndOthersUnchanged()
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
                    var preState = requirement.Helper.GetAllItems();
                    
                    var response = requirement.Creater.AddSome(secondHalf, allowedIdentity);
                    var postState = requirement.Helper.GetAllItems();

                    Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);
                    Assert.That(response.Items, Is.Not.Null);
                    Assert.That(requirement.Helper.ContainsNonIdentifiers(postState, secondHalf));
                    Assert.That(requirement.Helper.Contains(postState, preState));                    
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
                    var preState = requirement.Helper.GetAllItems();

                    var response = requirement.Creater.AddSome(secondHalf, deniedIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(preState), Is.True);
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
                    var preState = requirement.Helper.GetAllItems();

                    var response = requirement.Creater.AddSome(secondHalf, invalidIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(preState), Is.True);
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
                    var toAdd = new List<TPoco>(firstHalf);
                    toAdd.AddRange(secondHalf);
                    
                    var response = requirement.Creater.AddSome(toAdd, allowedIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.AddFailedCode));
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
                    var toAdd = new List<TPoco>(firstHalf);
                    toAdd.AddRange(secondHalf);

                    var response = requirement.Creater.AddSome(toAdd, deniedIdentity);

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
                    var toAdd = new List<TPoco>(firstHalf);
                    toAdd.AddRange(secondHalf);

                    var response = requirement.Creater.AddSome(toAdd, invalidIdentity);

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
                    var toAdd = new List<TPoco>(firstHalf);
                    toAdd.AddRange(secondHalf);

                    var response = requirement.Creater.AddSome(toAdd, allowedIdentity);

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
                    var toAdd = new List<TPoco>(firstHalf);
                    toAdd.AddRange(secondHalf);

                    var response = requirement.Creater.AddSome(toAdd, deniedIdentity);

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
                    var toAdd = new List<TPoco>(firstHalf);
                    toAdd.AddRange(secondHalf);

                    var response = requirement.Creater.AddSome(toAdd, invalidIdentity);

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
                    var toAdd = new List<TPoco>(secondHalf);
                    toAdd.AddRange(invalidItems);
                    var state = requirement.Helper.GetAllItems();

                    var response = requirement.Creater.AddSome(toAdd, allowedIdentity);

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
                    var toAdd = new List<TPoco>(secondHalf);
                    toAdd.AddRange(invalidItems);
                    var state = requirement.Helper.GetAllItems();

                    var response = requirement.Creater.AddSome(toAdd, deniedIdentity);

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
                    var toAdd = new List<TPoco>(secondHalf);
                    toAdd.AddRange(invalidItems);
                    var state = requirement.Helper.GetAllItems();

                    var response = requirement.Creater.AddSome(toAdd, invalidIdentity);

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
                    var response = requirement.Creater.AddSome(invalidItems, allowedIdentity);

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
                    var response = requirement.Creater.AddSome(invalidItems, deniedIdentity);

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
                    var response = requirement.Creater.AddSome(invalidItems, invalidIdentity);

                    Assert.That(requirement.Helper.HasNoItems(), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));                    
                }
            }
        }
        
        [Test]
        public virtual void IsEmpty_ItemsAllValidAllNew_IdentityIsAllowed_ReportsOkAndAllAdded()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                {
                    requirement.Helper.RemoveAllItems();

                    var validItems = requirement.Helper.MakeValidItems(TestSuitHelper.Keys.GenIfMnl);
                    var response = requirement.Creater.AddSome(validItems, allowedIdentity);
                    var stored = requirement.Helper.GetAllItems();

                    Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);
                    Assert.That(response.Items, Is.Not.Null);
                    Assert.That(requirement.Helper.NonIdentifiersAreEqual(validItems, response.Items));
                    Assert.That(requirement.Helper.AreEqual(response.Items, stored));
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
                    var response = requirement.Creater.AddSome(validItems, deniedIdentity);

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
                    var response = requirement.Creater.AddSome(validItems, invalidIdentity);

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
                    var toAdd = new List<TPoco>(validItems);
                    toAdd.AddRange(invalidItems);
                    
                    var response = requirement.Creater.AddSome(toAdd, allowedIdentity);

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
                    var toAdd = new List<TPoco>(validItems);
                    toAdd.AddRange(invalidItems);

                    var response = requirement.Creater.AddSome(toAdd, deniedIdentity);

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
                    var toAdd = new List<TPoco>(validItems);
                    toAdd.AddRange(invalidItems);

                    var response = requirement.Creater.AddSome(toAdd, invalidIdentity);

                    Assert.That(requirement.Helper.HasNoItems(), Is.True);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));                    
                }
            }
        }        
    }
}
