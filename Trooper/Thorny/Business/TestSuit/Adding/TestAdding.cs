
namespace Trooper.Thorny.Business.TestSuit.Adding
{
    using System;
    using NUnit.Framework;    
    using Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit;
    using Operation.Core;    

    public abstract class Adding<TPoco> : IAdding
        where TPoco : class
    {
        public abstract Func<AddingRequirment<TPoco>> Requirement { get; }
        
        [Test]
        public virtual void DoesAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsAllowed()
        {
            using (var requirment = this.Requirement())
            {
                requirment.Helper.RemoveAllItems();

                var item = requirment.Helper.MakeValidItem();

                Assert.IsNotNull(requirment.Helper.AddItem(item));
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsNotAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeInvalidIdentity();                
                var response = requirement.Creater.Add(item, identity);

                Assert.IsNull(response.Item);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsNull()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeValidItem();                
                var response = requirement.Creater.Add(item, null);
                                
                Assert.IsNull(response.Item);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemIsInvalidAndIdentityIsAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeValidIdentity();
                var response = requirement.Creater.Add(item, identity);

                Assert.IsNull(response.Item);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidPropertyCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemIsInvalidAndIdentityIsNotAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeInvalidIdentity();
                var response = requirement.Creater.Add(item, identity);

                Assert.IsNull(response.Item);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemIsInvalidAndIdentityIsNull()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeInvalidItem();
                var response = requirement.Creater.Add(item, null);

                Assert.IsNull(response.Item);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemIsNullAndIdentityIsAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var identity = requirement.Helper.MakeValidIdentity();
                var response = requirement.Creater.Add(null, identity);

                Assert.IsNull(response.Item);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemIsNullAndIdentityIsNotAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var identity = requirement.Helper.MakeInvalidIdentity();
                var response = requirement.Creater.Add(null, identity);

                Assert.IsNull(response.Item);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemIsNullAndIdentityIsNull()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var response = requirement.Creater.Add(null, null);

                Assert.IsNull(response.Item);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode));
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemAlreadyExistsAndIdentityIsAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeValidIdentity();
                var exitingItem = requirement.Helper.AddItem(item);
                var state = requirement.Helper.GetAllItems();

                requirement.Helper.ChangeNonIdentifiers(exitingItem);

                var addRresponse = requirement.Creater.Add(exitingItem, identity);

                Assert.IsNull(addRresponse.Item);
                Assert.That(requirement.Helper.ResponseFailsWithError(addRresponse, BusinessCore.AddFailedCode));
                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemAlreadyExistsAndIdentityIsNotAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeInvalidIdentity();
                var exitingItem = requirement.Helper.AddItem(item);
                var state = requirement.Helper.GetAllItems();

                requirement.Helper.ChangeNonIdentifiers(exitingItem);
                var addRresponse = requirement.Creater.Add(exitingItem, identity);

                Assert.IsNull(addRresponse.Item);
                Assert.That(requirement.Helper.ResponseFailsWithError(addRresponse, BusinessCore.UserDeniedCode));
                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }
        
        [Test]
        public virtual void DoesNotAddWhenItemAlreadyExistslAndIdentityIsNull()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeValidItem();
                var exitingItem = requirement.Helper.AddItem(item);
                var state = requirement.Helper.GetAllItems();

                requirement.Helper.ChangeNonIdentifiers(exitingItem);
                var addRresponse = requirement.Creater.Add(exitingItem, null);

                Assert.IsNull(addRresponse.Item);
                Assert.That(requirement.Helper.ResponseFailsWithError(addRresponse, BusinessCore.NullIdentityCode));
                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }
        
        [Test]
        public virtual void ItemDoesNotChangeWhenAddedItemAlreadyExistsAndIdentityIsAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeValidIdentity();
                var exitingItem = requirement.Helper.AddItem(item);
                var state = requirement.Helper.GetAllItems();

                requirement.Helper.ChangeNonIdentifiers(exitingItem);
                var addRresponse = requirement.Creater.Add(exitingItem, identity);

                Assert.IsNull(addRresponse.Item);
                Assert.That(requirement.Helper.ResponseFailsWithError(addRresponse, BusinessCore.AddFailedCode));
                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }
        
        [Test]
        public virtual void ItemDoesNotChangeWhenAddedItemAlreadyExistsAndIdentityIsNotAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeInvalidIdentity(); 
                var exitingItem = requirement.Helper.AddItem(item);
                var state = requirement.Helper.GetAllItems();

                requirement.Helper.ChangeNonIdentifiers(exitingItem);
                var addRresponse = requirement.Creater.Add(exitingItem, identity);

                Assert.IsNull(addRresponse.Item);
                Assert.That(requirement.Helper.ResponseFailsWithError(addRresponse, BusinessCore.UserDeniedCode));
                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }
        
        [Test]
        public virtual void ItemDoesNotChangeWhenAddedItemAlreadyExistslAndIdentityIsNull()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item = requirement.Helper.MakeValidItem();
                var exitingItem = requirement.Helper.AddItem(item);
                var state = requirement.Helper.GetAllItems();

                requirement.Helper.ChangeNonIdentifiers(exitingItem);
                var addRresponse = requirement.Creater.Add(exitingItem, null);

                Assert.IsNull(addRresponse.Item);
                Assert.That(requirement.Helper.ResponseFailsWithError(addRresponse, BusinessCore.NullIdentityCode));
                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
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
