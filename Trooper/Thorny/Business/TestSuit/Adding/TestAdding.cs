
namespace Trooper.Thorny.Business.TestSuit.Adding
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit;
    using Operation.Core;
    using System.Collections.Generic;
    using Trooper.Interface.Thorny.Business.Security;

    public abstract class Adding<TPoco>
        where TPoco : class
    {
        public abstract Func<AddingRequirment<TPoco>> Requirement { get; }

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsAllowed_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems())
                    foreach (var invalidItem in requirement.Helper.MakeInvalidItems())
                        foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                        {
                            requirement.Helper.RemoveAllItems();

                            var item1 = requirement.Helper.AddItem(validItem);
                            var item2 = requirement.Helper.AddItem(validItem);
                            var state = requirement.Helper.GetAllItems();
                            requirement.Helper.CopyIdentifiers(item2, invalidItem);

                            var response = requirement.Creater.Add(invalidItem, allowedIdentity);

                            Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                            Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                        }
            }
        }        

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsDenied_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems())
                    foreach (var invalidItem in requirement.Helper.MakeInvalidItems())
                        foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                        {
                            requirement.Helper.RemoveAllItems();

                            var item1 = requirement.Helper.AddItem(validItem);
                            var item2 = requirement.Helper.AddItem(validItem);
                            var state = requirement.Helper.GetAllItems();
                            requirement.Helper.CopyIdentifiers(item2, invalidItem);

                            var response = requirement.Creater.Add(invalidItem, deniedIdentity);

                            Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                            Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                        }
            }
        }        

        [Test]
        public virtual void HasItems_ItemIsInvalidExists_IdentityIsInvalid_ReportsErrorAndNoChange()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems())
                    foreach (var invalidItem in requirement.Helper.MakeInvalidItems())
                        foreach (var deniedIdentity in requirement.Helper.MakeInvalidIdentities())
                        {
                            requirement.Helper.RemoveAllItems();

                            var item1 = requirement.Helper.AddItem(validItem);
                            var item2 = requirement.Helper.AddItem(validItem);
                            var state = requirement.Helper.GetAllItems();
                            requirement.Helper.CopyIdentifiers(item2, invalidItem);

                            var response = requirement.Creater.Add(invalidItem, deniedIdentity);

                            Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                            Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                        }
            }
        }        
        
        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsAllowed_NoChangeAndReportsError()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems())
                    foreach (var invalidItem in requirement.Helper.MakeInvalidItems())
                        foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                        {
                            requirement.Helper.RemoveAllItems();

                            var item1 = requirement.Helper.AddItem(validItem);
                            var item2 = requirement.Helper.AddItem(validItem);
                            var state = requirement.Helper.GetAllItems();

                            var response = requirement.Creater.Add(invalidItem, allowedIdentity);

                            Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                            Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                        }
            }
        }        

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsDenied_NoChangeAndReportsError()
        {            
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems())
                    foreach (var invalidItem in requirement.Helper.MakeInvalidItems())
                        foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                        {
                            requirement.Helper.RemoveAllItems();

                            var item1 = requirement.Helper.AddItem(validItem);
                            var item2 = requirement.Helper.AddItem(validItem);
                            var state = requirement.Helper.GetAllItems();

                            var response = requirement.Creater.Add(invalidItem, deniedIdentity);

                            Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                            Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                        }
            }
        }        

        [Test]
        public virtual void HasItems_ItemIsInvalidNew_IdentityIsInvalid_NoChangeAndReportsError()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems())
                    foreach (var invalidItem in requirement.Helper.MakeInvalidItems())
                        foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                        {
                            requirement.Helper.RemoveAllItems();

                            var item1 = requirement.Helper.AddItem(validItem);
                            var item2 = requirement.Helper.AddItem(validItem);
                            var state = requirement.Helper.GetAllItems();

                            var response = requirement.Creater.Add(invalidItem, invalidIdentity);

                            Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                            Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                        }
            }
        }

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsAllowed_NoChangeAndReportsError()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems())
                    foreach (var existingValidItem in requirement.Helper.MakeValidItems())
                        foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                        {
                            requirement.Helper.RemoveAllItems();

                            var item1 = requirement.Helper.AddItem(validItem);
                            var item2 = requirement.Helper.AddItem(validItem);
                            var state = requirement.Helper.GetAllItems();
                            requirement.Helper.CopyIdentifiers(item2, existingValidItem);
                            requirement.Helper.ChangeNonIdentifiers(existingValidItem);

                            var response = requirement.Creater.Add(existingValidItem, allowedIdentity);

                            Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                            Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.AddFailedCode));
                        }
            }
        }       

        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsDenied_NoChangeAndReportsError()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems())
                    foreach (var existingValidItem in requirement.Helper.MakeValidItems())
                        foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                        {
                            requirement.Helper.RemoveAllItems();

                            var item1 = requirement.Helper.AddItem(validItem);
                            var item2 = requirement.Helper.AddItem(validItem);
                            var state = requirement.Helper.GetAllItems();
                            requirement.Helper.CopyIdentifiers(item2, existingValidItem);
                            requirement.Helper.ChangeNonIdentifiers(existingValidItem);

                            var response = requirement.Creater.Add(existingValidItem, deniedIdentity);

                            Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                            Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                        }
            }
        }
        
        [Test]
        public virtual void HasItems_ItemIsValidExists_IdentityIsInvalid_NoChangeAndReportsError()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems())
                    foreach (var existingValidItem in requirement.Helper.MakeValidItems())
                        foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                        {
                            requirement.Helper.RemoveAllItems();

                            var item1 = requirement.Helper.AddItem(validItem);
                            var item2 = requirement.Helper.AddItem(validItem);
                            var state = requirement.Helper.GetAllItems();
                            requirement.Helper.CopyIdentifiers(item2, existingValidItem);
                            requirement.Helper.ChangeNonIdentifiers(existingValidItem);

                            var response = requirement.Creater.Add(existingValidItem, invalidIdentity);

                            Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                            Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                        }
            }
        }        

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsAllowed_IsAddedAndOthersUnchangedAndReportsOk()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems())
                    foreach (var newValidItem in requirement.Helper.MakeValidItems())
                        foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                        {
                            requirement.Helper.RemoveAllItems();

                            var item1 = requirement.Helper.AddItem(validItem);
                            var item2 = requirement.Helper.AddItem(validItem);
                            var preState = requirement.Helper.GetAllItems();                                                        

                            var response = requirement.Creater.Add(newValidItem, allowedIdentity);

                            Assert.That(response, Is.Not.Null);
                            Assert.That(response.Item, Is.Not.Null);
                            Assert.That(requirement.Helper.ItemCountIs(3));
                            Assert.That(requirement.Helper.NonIdentifersAreEqual(newValidItem, response.Item));

                            var postState = requirement.Helper.GetAllItems();

                            Assert.That(postState.Any(i => requirement.Helper.AreEqual(i, item1)));
                            Assert.That(postState.Any(i => requirement.Helper.AreEqual(i, item2)));

                            Assert.That(response.Ok);
                        }
            }
        }                

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsDenied_NoChangeAndReportsError()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems())
                    foreach (var newValidItem in requirement.Helper.MakeValidItems())
                        foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                        {
                            requirement.Helper.RemoveAllItems();

                            var item1 = requirement.Helper.AddItem(validItem);
                            var item2 = requirement.Helper.AddItem(validItem);
                            var state = requirement.Helper.GetAllItems();

                            var response = requirement.Creater.Add(newValidItem, deniedIdentity);

                            Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                            Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                        }
            }
        }        

        [Test]
        public virtual void HasItems_ItemIsValidNew_IdentityIsInvalid_NoChangeAndReportsError()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var validItem in requirement.Helper.MakeValidItems())
                    foreach (var newValidItem in requirement.Helper.MakeValidItems())
                        foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                        {
                            requirement.Helper.RemoveAllItems();

                            var item1 = requirement.Helper.AddItem(validItem);
                            var item2 = requirement.Helper.AddItem(validItem);
                            var state = requirement.Helper.GetAllItems();

                            var response = requirement.Creater.Add(newValidItem, invalidIdentity);

                            Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
                            Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
                        }
            }
        }        
        
        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsAllowed_NoChangeAndReportsError()
        {
            using (var requirement = this.Requirement())
            {                
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems())
                    foreach (var allowedIdentity in requirement.Helper.MakeAllowedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Creater.Add(invalidItem, allowedIdentity);

                        Assert.That(requirement.Helper.ItemCountIs(0));
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidDataCode));
                    }
            }
        }        

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsDenied_NoChangeAndReportsError()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems())
                    foreach (var deniedIdentity in requirement.Helper.MakeDeniedIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Creater.Add(invalidItem, deniedIdentity);

                        Assert.That(requirement.Helper.ItemCountIs(0));
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                    }
            }
        }        

        [Test]
        public virtual void IsEmpty_ItemIsInvalidNew_IdentityIsInvalid_NoChangeAndReportsError()
        {
            using (var requirement = this.Requirement())
            {
                foreach (var invalidItem in requirement.Helper.MakeInvalidItems())
                    foreach (var invalidIdentity in requirement.Helper.MakeInvalidIdentities())
                    {
                        requirement.Helper.RemoveAllItems();

                        var response = requirement.Creater.Add(invalidItem, invalidIdentity);

                        Assert.That(requirement.Helper.ItemCountIs(0));
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
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

                        Assert.That(requirement.Helper.ItemCountIs(0));
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

                        Assert.That(requirement.Helper.ItemCountIs(0));
                        Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidIdentityCode));
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
