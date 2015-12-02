
namespace Trooper.Thorny.Business.TestSuit.Adding
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit;
    using Operation.Core;
    using System.Collections.Generic;
    using Trooper.Interface.Thorny.Business.Security;

    public abstract class TestAdding<TPoco>
        where TPoco : class, new()
    {
        public abstract Func<AddingRequirment<TPoco>> Requirement { get; }

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

                        var response = requirement.Creater.Add(item, allowedIdentity);

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

                        var response = requirement.Creater.Add(item, deniedIdentity);

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

                        var response = requirement.Creater.Add(item, invalidIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);

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
        }        
        
        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems())
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        requirement.Helper.AddValidItems();
                        var state = requirement.Helper.GetAllItems();

                        var response = requirement.Creater.Add(invalidItem, allowedIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);

                        if (invalidItem == null)
                        {
                            Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode));
                        }
                        else
                        {
                            Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                        }                        
                    }
            }
        }        

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems())
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        requirement.Helper.AddValidItems();
                        var state = requirement.Helper.GetAllItems();

                        var response = requirement.Creater.Add(invalidItem, deniedIdentity);

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
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems())
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        requirement.Helper.AddValidItems();
                        var state = requirement.Helper.GetAllItems();

                        var response = requirement.Creater.Add(invalidItem, invalidIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);

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
        public virtual void HasItems_ItemIsValidExists_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var existingValidItem in requirement.Helper.MakeValidItems())
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var validItems = requirement.Helper.AddValidItems();
                        var item = validItems.Last();
                        var state = requirement.Helper.GetAllItems();

                        requirement.Helper.CopyIdentifiers(item, existingValidItem);
                        requirement.Helper.ChangeNonIdentifiers(existingValidItem, validItems, true);

                        var response = requirement.Creater.Add(existingValidItem, allowedIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.AddFailedCode));
                    }
            }
        }       

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var existingValidItem in requirement.Helper.MakeValidItems())
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var validItems = requirement.Helper.AddValidItems();
                        var item = validItems.Last();
                        var state = requirement.Helper.GetAllItems();

                        requirement.Helper.CopyIdentifiers(item, existingValidItem);
                        requirement.Helper.ChangeNonIdentifiers(existingValidItem, validItems, true);

                        var response = requirement.Creater.Add(existingValidItem, deniedIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
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
                foreach (var existingValidItem in requirement.Helper.MakeValidItems())
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var validItems = requirement.Helper.AddValidItems();
                        var item = validItems.Last();
                        var state = requirement.Helper.GetAllItems();

                        requirement.Helper.CopyIdentifiers(item, existingValidItem);
                        requirement.Helper.ChangeNonIdentifiers(existingValidItem, validItems, true);

                        var response = requirement.Creater.Add(existingValidItem, invalidIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);

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
        public virtual void HasItems_ItemIsValidNew_IdentityIsAllowed_ReportsOkAndIsAddedAndOthersUnchanged()
        {
            using (var requirement = this.Requirement())
            {
                var validItems = requirement.Helper.MakeValidItems().ToList();
                
                for (var target = 0; target<validItems.Count(); target++)
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();
                        var items = new List<TPoco>();

                        for (var i=0; i< validItems.Count(); i++)
                        {
                            if (i != target)
                            {
                                var copy = requirement.Helper.Copy(validItems[target]);
                                items.Add(copy);
                            }
                        }
                        
                        var preState = requirement.Helper.AddItems(items);
                        var response = requirement.Creater.Add(validItems.ElementAt(target), allowedIdentity);
                        var postState = requirement.Helper.GetAllItems();

                        Assert.That(requirement.Helper.ResponseIsOk(response), Is.True);
                        Assert.That(response.Item, Is.Not.Null);
                        Assert.That(requirement.Helper.ItemCountIs(validItems.Count()));
                        Assert.That(requirement.Helper.NonIdentifiersAreEqual(validItems.ElementAt(target), response.Item));
                        Assert.That(requirement.Helper.Contains(postState, preState), Is.True);
                        Assert.That(requirement.Helper.ContainsNonIdentifiers(validItems, postState), Is.True);
                    }
            }
        }                

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var newValidItem in requirement.Helper.MakeValidItems())
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        requirement.Helper.AddValidItems();
                        var state = requirement.Helper.GetAllItems();

                        var response = requirement.Creater.Add(newValidItem, deniedIdentity);

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
                foreach (var newValidItem in requirement.Helper.MakeValidItems())
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        requirement.Helper.AddValidItems();
                        var state = requirement.Helper.GetAllItems();

                        var response = requirement.Creater.Add(newValidItem, invalidIdentity);

                        Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                        Assert.That(response.Item, Is.Null);

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
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {                
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems())
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Creater.Add(invalidItem, allowedIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);

                        if (invalidItem == null)
                        {
                            Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode));
                        }
                        else
                        {
                            Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                        }
                    }
            }
        }        

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems())
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Creater.Add(invalidItem, deniedIdentity);

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
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems())
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Creater.Add(invalidItem, invalidIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);

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
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsAllowed_ReportsOkAndIsAdded()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems())
                    foreach (var validIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Creater.Add(validItem, validIdentity);
                        var items = requirement.Helper.GetAllItems();

                        Assert.That(response, Is.Not.Null);
                        Assert.That(response.Item, Is.Not.Null);
                        Assert.That(response.Ok);
                        Assert.That(items.Count, Is.EqualTo(1));
                        Assert.That(requirement.Helper.AreEqual(items.First(), response.Item));
                    }
            }            
        }

        [Test]
        public virtual void IsEmpty_ItemIsValidNew_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems())
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Creater.Add(validItem, deniedIdentity);

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
                foreach (var validItem in requirement.Helper.MakeValidItems())
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Creater.Add(validItem, invalidIdentity);

                        Assert.That(requirement.Helper.HasNoItems(), Is.True);
                        Assert.That(response.Item, Is.Null);

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
        public void SelfTestHelper()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.SelfTestHelper();
            }
        }
    }
}
