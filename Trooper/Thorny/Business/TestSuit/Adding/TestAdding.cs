using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Trooper.Thorny.Business.TestSuit.Adding
{
    using FluentAssertions;

    using NUnit.Framework;

    using Trooper.Interface.Thorny.Business.Operation.Single;
    using Trooper.Interface.Thorny.TestSuit;
    using Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit;
    using Trooper.Thorny.Business.Operation.Core;    

    public abstract class Adding<TPoco> : IAdding
        where TPoco : class
    {
        public abstract Func<AddingRequirment<TPoco>> Requirement { get; }
        
        [Test]
        public virtual void DoesAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsAllowed()
        {
            using (var requirment = this.Requirement())
            {
                requirment.Helper.RemoveAllItems(requirment.Reader, requirment.Deleter);

                var item = requirment.Helper.MakeValidItem();

                requirment.Helper.AddItem(item, requirment.Creater, requirment.Reader);
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsNotAllowed()
        {
            using (var requirment = this.Requirement())
            {
                requirment.Helper.RemoveAllItems(requirment.Reader, requirment.Deleter);

                var item = requirment.Helper.MakeValidItem();
                var identity = requirment.Helper.MakeInvalidIdentity();                
                var response = requirment.Creater.Add(item, identity);

                Assert.IsNull(response.Item);
                requirment.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode);
                requirment.Helper.NoItemsExist(requirment.Reader);
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsNull()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems(requirement.Reader, requirement.Deleter);

                var item = requirement.Helper.MakeValidItem();                
                var response = requirement.Creater.Add(item, null);
                                
                Assert.IsNull(response.Item);
                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode);
                requirement.Helper.NoItemsExist(requirement.Reader);
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemIsInvalidAndIdentityIsAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems(requirement.Reader, requirement.Deleter);

                var item = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeValidIdentity();
                var response = requirement.Creater.Add(item, identity);

                Assert.IsNull(response.Item);
                requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidPropertyCode);
                requirement.Helper.NoItemsExist(requirement.Reader);
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemIsInvalidAndIdentityIsNotAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems(requirement.Reader, requirement.Deleter);

                var item = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeInvalidIdentity();
                var response = requirement.Creater.Add(item, identity);

                Assert.IsNull(response.Item);
                requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode);
                requirement.Helper.NoItemsExist(requirement.Reader);
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemIsInvalidAndIdentityIsNull()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems(requirement.Reader, requirement.Deleter);

                var item = requirement.Helper.MakeInvalidItem();
                var response = requirement.Creater.Add(item, null);

                Assert.IsNull(response.Item);
                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode);
                requirement.Helper.NoItemsExist(requirement.Reader);
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemIsNullAndIdentityIsAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems(requirement.Reader, requirement.Deleter);

                var identity = requirement.Helper.MakeValidIdentity();
                var response = requirement.Creater.Add(null, identity);

                Assert.IsNull(response.Item);
                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode);
                requirement.Helper.NoItemsExist(requirement.Reader);
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemIsNullAndIdentityIsNotAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems(requirement.Reader, requirement.Deleter);

                var identity = requirement.Helper.MakeInvalidIdentity();
                var response = requirement.Creater.Add(null, identity);

                Assert.IsNull(response.Item);
                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode);
                requirement.Helper.NoItemsExist(requirement.Reader);
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemIsNullAndIdentityIsNull()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems(requirement.Reader, requirement.Deleter);

                var response = requirement.Creater.Add(null, null);

                Assert.IsNull(response.Item);
                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode);
                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode);
                requirement.Helper.NoItemsExist(requirement.Reader);
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemAlreadyExistsAndIdentityIsAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems(requirement.Reader, requirement.Deleter);

                var item = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeValidIdentity();
                var exitingItem = requirement.Helper.AddItem(item, requirement.Creater, requirement.Reader);

                requirement.Helper.ChangeNonIdentifiers(exitingItem);

                var addRresponse = requirement.Creater.Add(exitingItem, identity);
                Assert.IsNull(addRresponse.Item);
                requirement.Helper.ResponseFailsWithError(addRresponse, BusinessCore.AddFailedCode);
                
                Assert.That(requirement.Helper.ItemCountIs(1, requirement.Reader));
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemAlreadyExistsAndIdentityIsNotAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems(requirement.Reader, requirement.Deleter);

                var item = requirement.Helper.MakeValidItem();
                var validIdentity = requirement.Helper.MakeValidIdentity();
                var invalidIdentity = requirement.Helper.MakeInvalidIdentity();
                var exitingItem = requirement.Helper.AddItem(item, requirement.Creater, requirement.Reader);

                requirement.Helper.ChangeNonIdentifiers(exitingItem);

                var addRresponse = requirement.Creater.Add(exitingItem, invalidIdentity);
                Assert.IsNull(addRresponse.Item);
                requirement.Helper.ResponseFailsWithError(addRresponse, BusinessCore.UserDeniedCode);

                Assert.That(requirement.Helper.ItemCountIs(1, requirement.Reader));
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemAlreadyExistslAndIdentityIsNull()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems(requirement.Reader, requirement.Deleter);

                var item = requirement.Helper.MakeValidItem();
                var validIdentity = requirement.Helper.MakeValidIdentity();
                var exitingItem = requirement.Helper.AddItem(item, requirement.Creater, requirement.Reader);

                requirement.Helper.ChangeNonIdentifiers(exitingItem);

                var addRresponse = requirement.Creater.Add(exitingItem, null);
                Assert.IsNull(addRresponse.Item);
                requirement.Helper.ResponseFailsWithError(addRresponse, BusinessCore.NullIdentityCode);

                Assert.That(requirement.Helper.ItemCountIs(1, requirement.Reader));
            }
        }
        
        [Test]
        public virtual void ItemDoesNotChangeWhenAddedItemAlreadyExistsAndIdentityIsAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems(requirement.Reader, requirement.Deleter);

                var item = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeValidIdentity();
                var exitingItem = requirement.Helper.AddItem(item, requirement.Creater, requirement.Reader);

                requirement.Helper.ChangeNonIdentifiers(exitingItem);

                var addRresponse = requirement.Creater.Add(exitingItem, identity);
                Assert.IsNull(addRresponse.Item);
                requirement.Helper.ResponseFailsWithError(addRresponse, BusinessCore.AddFailedCode);

                var retrievedItem = requirement.Helper.GetItem(exitingItem, requirement.Reader);
                Assert.That(!requirement.Helper.AreEqual(retrievedItem, exitingItem));
            }
        }
        
        [Test]
        public virtual void ItemDoesNotChangeWhenAddedItemAlreadyExistsAndIdentityIsNotAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems(requirement.Reader, requirement.Deleter);

                var item = requirement.Helper.MakeValidItem();
                var validIdentity = requirement.Helper.MakeValidIdentity();
                var invalidIdentity = requirement.Helper.MakeInvalidIdentity();
                var exitingItem = requirement.Helper.AddItem(item, requirement.Creater, requirement.Reader);

                requirement.Helper.ChangeNonIdentifiers(exitingItem);

                var addRresponse = requirement.Creater.Add(exitingItem, invalidIdentity);
                Assert.IsNull(addRresponse.Item);
                requirement.Helper.ResponseFailsWithError(addRresponse, BusinessCore.UserDeniedCode);

                Assert.That(requirement.Helper.ItemCountIs(1, requirement.Reader));
            }
        }
        
        [Test]
        public virtual void ItemDoesNotChangeWhenAddedItemAlreadyExistslAndIdentityIsNull()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems(requirement.Reader, requirement.Deleter);

                var item = requirement.Helper.MakeValidItem();
                var validIdentity = requirement.Helper.MakeValidIdentity();
                var exitingItem = requirement.Helper.AddItem(item, requirement.Creater, requirement.Reader);

                requirement.Helper.ChangeNonIdentifiers(exitingItem);

                var addRresponse = requirement.Creater.Add(exitingItem, null);
                Assert.IsNull(addRresponse.Item);
                requirement.Helper.ResponseFailsWithError(addRresponse, BusinessCore.NullIdentityCode);

                var retrievedItem = requirement.Helper.GetItem(exitingItem, requirement.Reader);
                Assert.That(!requirement.Helper.AreEqual(retrievedItem, exitingItem));
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
