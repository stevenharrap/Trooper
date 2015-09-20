using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit;
using Trooper.Thorny.Business.Operation.Core;

namespace Trooper.Thorny.Business.TestSuit.Adding
{
    public abstract class TestAddingSome<TPoco> : IAddingSome
        where TPoco : class
    {
        public abstract Func<AddingRequirment<TPoco>> Requirement { get; }

        [Test]
        public void DoesAddWhenAllItemsDoNotExistAndAreValidAndIdentityIsAllowed()
        {
            using (var requirment = this.Requirement())
            {
                requirment.Helper.RemoveAllItems();

                var item1 = requirment.Helper.MakeValidItem();
                var item2 = requirment.Helper.MakeValidItem();
                var identity = requirment.Helper.MakeValidIdentity();

                Assert.IsNotNull(requirment.Helper.AddItems(new List<TPoco>{ item1, item2 }));
            }
        }

        [Test]
        public void DoesNotAddWhenAllItemsDoNotExistAndAreAreValidAndIdentityIsNotAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.MakeValidItem();
                var item2 = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeInvalidIdentity();
                var response = requirement.Creater.AddSome(new List<TPoco> { item1, item2 }, identity);

                Assert.IsNull(response.Items);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }

        [Test]
        public void DoesNotAddWhenAllItemsDoNotExistAndAreValidAndIdentityIsNull()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.MakeValidItem();
                var item2 = requirement.Helper.MakeValidItem();
                var response = requirement.Creater.AddSome(new List<TPoco> { item1, item2 }, null);

                Assert.IsNull(response.Items);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }

        [Test]
        public void DoesNotAddWhenAnyItemAlreadyExistsAndIdentityIsAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.MakeValidItem();                
                var identity = requirement.Helper.MakeValidIdentity();
                var addedItem1 = requirement.Helper.AddItem(item1);
                var item2 = requirement.Helper.CopyAndChangeItemNonIdentifiers(addedItem1);
                var item3 = requirement.Helper.MakeInvalidItem();
                var state = requirement.Helper.GetAllItems();

                var response = requirement.Creater.AddSome(new List<TPoco> { item2, item3 }, identity);

                Assert.IsNull(response.Items);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.AddFailedCode));
                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public void DoesNotAddWhenAnyItemAlreadyExistsAndIdentityIsNotAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.MakeValidItem();                
                var identity = requirement.Helper.MakeInvalidIdentity();
                var addedItem1 = requirement.Helper.AddItem(item1);
                var item2 = requirement.Helper.CopyAndChangeItemNonIdentifiers(addedItem1);
                var item3 = requirement.Helper.MakeInvalidItem();
                var state = requirement.Helper.GetAllItems();

                var response = requirement.Creater.AddSome(new List<TPoco> { item2, item3 }, identity);

                Assert.IsNull(response.Items);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public void DoesNotAddWhenAnyItemAlreadyExistslAndIdentityIsNull()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.MakeValidItem();
                var addedItem1 = requirement.Helper.AddItem(item1);
                var item2 = requirement.Helper.CopyAndChangeItemNonIdentifiers(addedItem1);
                var item3 = requirement.Helper.MakeInvalidItem();
                var state = requirement.Helper.GetAllItems();

                var response = requirement.Creater.AddSome(new List<TPoco> { item2, item3 }, null);

                Assert.IsNull(response.Items);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode));
                Assert.That(requirement.Helper.StoredItemsAreEqualTo(state));
            }
        }

        [Test]
        public void DoesNotAddWhenAnyItemsAreInvalidAndIdentityIsAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.MakeValidItem();
                var item2 = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeValidIdentity();

                var response = requirement.Creater.AddSome(new List<TPoco> { item1, item2 }, identity);

                Assert.IsNull(response.Items);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.AddFailedCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }

        [Test]
        public void DoesNotAddWhenAnyItemsAreInvalidAndIdentityIsNotAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.MakeValidItem();
                var item2 = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeInvalidIdentity();

                var response = requirement.Creater.AddSome(new List<TPoco> { item1, item2 }, identity);

                Assert.IsNull(response.Items);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }

        [Test]
        public void DoesNotAddWhenAnyItemsAreInvalidAndIdentityIsNull()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.MakeValidItem();
                var item2 = requirement.Helper.MakeInvalidItem();

                var response = requirement.Creater.AddSome(new List<TPoco> { item1, item2 }, null);

                Assert.IsNull(response.Items);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }

        [Test]
        public void DoesNotAddWhenAnyItemsAreNullAndIdentityIsAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeValidIdentity();

                var response = requirement.Creater.AddSome(new List<TPoco> { item1, null }, identity);

                Assert.IsNull(response.Items);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }

        [Test]
        public void DoesNotAddWhenAnyItemsAreNullAndIdentityIsNotAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.MakeValidItem();
                var identity = requirement.Helper.MakeValidIdentity();

                var response = requirement.Creater.AddSome(new List<TPoco> { item1, null }, identity);

                Assert.IsNull(response.Items);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }

        [Test]
        public void DoesNotAddWhenAnyItemsAreNullAndIdentityIsNull()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var item1 = requirement.Helper.MakeValidItem();

                var response = requirement.Creater.AddSome(new List<TPoco> { item1, null }, null);

                Assert.IsNull(response.Items);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }

        [Test]
        public void DoesNotAddWhenItemsIsNullAndIdentityIsAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var identity = requirement.Helper.MakeValidIdentity();

                var response = requirement.Creater.AddSome(null, identity);

                Assert.IsNull(response.Items);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }

        [Test]
        public void DoesNotAddWhenItemsIsNullAndIdentityIsNotAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var identity = requirement.Helper.MakeInvalidIdentity();

                var response = requirement.Creater.AddSome(null, identity);

                Assert.IsNull(response.Items);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode));
                Assert.That(requirement.Helper.NoItemsExist());
            }
        }

        [Test]
        public void DoesNotAddWhenItemsIsNullAndIdentityIsNull()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems();

                var response = requirement.Creater.AddSome(null, null);

                Assert.IsNull(response.Items);
                Assert.That(requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullDataCode));
                Assert.That(requirement.Helper.NoItemsExist());
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
