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
        public abstract Func<AddingRequirment<TPoco>> Requirement { get; }

        [Test]
        public virtual void HasItems_ItemsAllInvalidAllExist_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems().Where(i => i != null))
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var willBeInvalidItems = requirement.Helper.AddValidItems();
                        var state = requirement.Helper.GetAllItems();

                        foreach (var item in willBeInvalidItems)
                        {
                            requirement.Helper.CopyNonIdentifiers(invalidItem, item);
                        }

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
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems().Where(i => i != null))
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var willBeInvalidItems = requirement.Helper.AddValidItems();
                        var state = requirement.Helper.GetAllItems();

                        foreach (var item in willBeInvalidItems)
                        {
                            requirement.Helper.CopyNonIdentifiers(invalidItem, item);
                        }

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
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems().Where(i => i != null))
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var willBeInvalidItems = requirement.Helper.AddValidItems();
                        var state = requirement.Helper.GetAllItems();

                        foreach (var item in willBeInvalidItems)
                        {
                            requirement.Helper.CopyNonIdentifiers(invalidItem, item);
                        }

                        var response = requirement.Creater.AddSome(willBeInvalidItems, invalidIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Items, Is.Null);

                        if (invalidIdentity == null)
                        {
                            Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode));
                        }
                        else
                        {
                            Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                        }                        
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

                    var invalidItems = requirement.Helper.MakeInvalidItems();
                    var state = requirement.Helper.GetAllItems();

                    var response = requirement.Creater.AddSome(invalidItems, allowedIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(response.Items, Is.Null);
                    Assert.That(
                        requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode)
                        || requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode));
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

                    var invalidItems = requirement.Helper.MakeInvalidItems();
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

                    var invalidItems = requirement.Helper.MakeInvalidItems();
                    var state = requirement.Helper.GetAllItems();

                    var response = requirement.Creater.AddSome(invalidItems, invalidIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(response.Items, Is.Null);

                    if (invalidIdentity == null)
                    {
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode));
                    }
                    else
                    {
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
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

                    var validItems = requirement.Helper.AddValidItems(2).ToList();
                    var item1 = validItems[0];
                    var item2 = validItems[1];
                    var state = requirement.Helper.GetAllItems();

                    var invalidItems = requirement.Helper.MakeInvalidItems(4, false).ToList();
                    requirement.Helper.CopyIdentifiers(item1, invalidItems[0]);
                    requirement.Helper.CopyIdentifiers(item2, invalidItems[1]);

                    var response = requirement.Creater.AddSome(invalidItems, allowedIdentity);

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

                    var validItems = requirement.Helper.AddValidItems(2).ToList();
                    var item1 = validItems[0];
                    var item2 = validItems[1];
                    var state = requirement.Helper.GetAllItems();

                    var invalidItems = requirement.Helper.MakeInvalidItems(4, false).ToList();
                    requirement.Helper.CopyIdentifiers(item1, invalidItems[0]);
                    requirement.Helper.CopyIdentifiers(item2, invalidItems[1]);

                    var response = requirement.Creater.AddSome(invalidItems, deniedIdentity);

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

                    var validItems = requirement.Helper.AddValidItems(2).ToList();
                    var item1 = validItems[0];
                    var item2 = validItems[1];
                    var state = requirement.Helper.GetAllItems();

                    var invalidItems = requirement.Helper.MakeInvalidItems(4, false).ToList();
                    requirement.Helper.CopyIdentifiers(item1, invalidItems[0]);
                    requirement.Helper.CopyIdentifiers(item2, invalidItems[1]);

                    var response = requirement.Creater.AddSome(invalidItems, invalidIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                    Assert.That(response.Items, Is.Null);

                    if (invalidIdentity == null)
                    {
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode));
                    }
                    else
                    {
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
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

                    var response = requirement.Creater.AddSome(validItems, invalidIdentity);
                    Assert.That(response.Items, Is.Null);
                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));

                    if (invalidIdentity == null)
                    {
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode));
                    }
                    else
                    {
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
                }
            }
        }

        [Test]
        public virtual void HasItems_ItemsAllValidAllNew_IdentityIsAllowed_ReportsOkAndAllAddedAndOthersUnchanged()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                {
                    requirement.Helper.RemoveAllItems();
                    
                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredInvalidItems * 2).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredInvalidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredInvalidItems, requirement.Helper.DefaultRequiredInvalidItems);

                    requirement.Helper.AddItems(firstHalf);
                    var preState = requirement.Helper.GetAllItems();
                    
                    var response = requirement.Creater.AddSome(secondHalf, invalidIdentity);
                    var postState = requirement.Helper.GetAllItems();

                    Assert.That(requirement.Helper.ResponseIsOk(response));
                    Assert.That(response.Items, Is.Not.Null);
                    Assert.That(response.Items, Is.EqualTo(requirement.Helper.DefaultRequiredInvalidItems * 2));

                    for (var i = 0; i < requirement.Helper.DefaultRequiredInvalidItems; i++)
                    {
                        var preStateItem = preState[i];
                        Assert.That(postState.Count(item => requirement.Helper.AreEqual(preStateItem, item)), Is.EqualTo(1));
                    }

                    for (var i = 0; i < requirement.Helper.DefaultRequiredInvalidItems; i++)
                    {
                        var item = secondHalf[i];
                        Assert.That(response.Items.Count(addedItem => requirement.Helper.NonIdentifersAreEqual(addedItem, item)), Is.EqualTo(1));
                    }
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

                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredInvalidItems * 2).ToList();
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

                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredInvalidItems * 2).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredInvalidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredInvalidItems, requirement.Helper.DefaultRequiredInvalidItems);

                    requirement.Helper.AddItems(firstHalf);
                    var preState = requirement.Helper.GetAllItems();

                    var response = requirement.Creater.AddSome(secondHalf, invalidIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(preState), Is.True);
                    Assert.That(response.Items, Is.Null);

                    if (invalidIdentity == null)
                    {
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode));
                    }
                    else
                    {
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
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

                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredInvalidItems * 2).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredInvalidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredInvalidItems, requirement.Helper.DefaultRequiredInvalidItems);

                    firstHalf = requirement.Helper.AddItems(firstHalf).ToList();
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

                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredInvalidItems * 2).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredInvalidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredInvalidItems, requirement.Helper.DefaultRequiredInvalidItems);

                    firstHalf = requirement.Helper.AddItems(firstHalf).ToList();
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

                    var validItems = requirement.Helper.MakeValidItems(requirement.Helper.DefaultRequiredInvalidItems * 2).ToList();
                    var firstHalf = validItems.GetRange(0, requirement.Helper.DefaultRequiredInvalidItems);
                    var secondHalf = validItems.GetRange(requirement.Helper.DefaultRequiredInvalidItems, requirement.Helper.DefaultRequiredInvalidItems);

                    firstHalf = requirement.Helper.AddItems(firstHalf).ToList();
                    var state = requirement.Helper.GetAllItems();
                    var toAdd = new List<TPoco>(firstHalf);
                    toAdd.AddRange(secondHalf);

                    var response = requirement.Creater.AddSome(toAdd, invalidIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    Assert.That(response.Items, Is.Null);

                    if (invalidIdentity == null)
                    {
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode));
                    }
                    else
                    {
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                    }
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

                    var invalidItems = requirement.Helper.MakeInvalidItems(false).ToList();
                    var items = requirement.Helper.AddValidItems(requirement.Helper.DefaultRequiredValidItems + invalidItems.Count());
                    var state = requirement.Helper.GetAllItems();

                    for (var i = 0; i <items.Count(); i++)
                    {
                        requirement.Helper.CopyNonIdentifiers(invalidItems[i], items[i]);
                    }                    

                    var response = requirement.Creater.AddSome(items, allowedIdentity);

                    Assert.That(requirement.Helper.StoredItemsAreEqualTo(state), Is.True);
                    Assert.That(response.Items, Is.Null);                    
                    Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                }
            }
        }


        [Test]
        [Ignore("Todo")]
        public virtual void HasItems_ItemsSomeValidAllExist_IdentityIsDenied_ReportsErrorAndNoChange() {  }

        [Test]
        [Ignore("Todo")]
        public virtual void HasItems_ItemsSomeValidAllExist_IdentityIsInvalid_ReportsErrorAndNoChange() {  }

        [Test]
        [Ignore("Todo")]
        public virtual void HasItems_ItemsSomeValidAllNew_IdentityIsAllowed_ReportsErrorAndNoChange() {  }

        [Test]
        [Ignore("Todo")]
        public virtual void HasItems_ItemsSomeValidAllNew_IdentityIsDenied_ReportsErrorAndNoChange() {  }

        [Test]
        [Ignore("Todo")]
        public virtual void HasItems_ItemsSomeValidAllNew_IdentityIsInvalid_ReportsErrorAndNoChange() {  }

        [Test]
        [Ignore("Todo")]
        public virtual void IsEmpty_ItemsAllInvalidAllNew_IdentityIsAllowed_ReportsErrorAndNoChange() {  }

        [Test]
        [Ignore("Todo")]
        public virtual void IsEmpty_ItemsAllInvalidAllNew_IdentityIsDenied_ReportsErrorAndNoChange() {  }

        [Test]
        [Ignore("Todo")]
        public virtual void IsEmpty_ItemsAllInvalidAllNew_IdentityIsInvalid_ReportsErrorAndNoChange() {  }

        [Test]
        [Ignore("Todo")]
        public virtual void IsEmpty_ItemsAllValidAllNew_IdentityIsAllowed_ReportsErrorAndNoChange() {  }

        [Test]
        [Ignore("Todo")]
        public virtual void IsEmpty_ItemsAllValidAllNew_IdentityIsAllowed_ReportsOkAndAllAdded() {  }

        [Test]
        [Ignore("Todo")]
        public virtual void IsEmpty_ItemsAllValidAllNew_IdentityIsDenied_ReportsErrorAndNoChange() {  }

        [Test]
        [Ignore("Todo")]
        public virtual void IsEmpty_ItemsAllValidAllNew_IdentityIsInvalid_ReportsErrorAndNoChange() {  }

        [Test]
        [Ignore("Todo")]
        public virtual void IsEmpty_ItemsSomeValidAllNew_IdentityIsAllowed_ReportsErrorAndNoChange() {  }

        [Test]
        [Ignore("Todo")]
        public virtual void IsEmpty_ItemsSomeValidAllNew_IdentityIsDenied_ReportsErrorAndNoChange() {  }

        [Test]
        [Ignore("Todo")]
        public virtual void IsEmpty_ItemsSomeValidAllNew_IdentityIsInvalid_ReportsErrorAndNoChange() {  }

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
