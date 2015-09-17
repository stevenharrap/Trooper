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
                requirment.Helper.RemoveAllItems(requirment.Reader, requirment.Deleter);

                var item1 = requirment.Helper.MakeValidItem();
                var item2 = requirment.Helper.MakeValidItem();
                var identity = requirment.Helper.MakeValidIdentity();

                requirment.Helper.AddItems(new List<TPoco>{ item1, item2 }, requirment.Creater, requirment.Reader);
            }
        }

        [Test]
        public void DoesNotAddWhenAllItemsDoNotExistAndAreAreValidAndIdentityIsNotAllowed()
        {
            using (var requirment = this.Requirement())
            {
                requirment.Helper.RemoveAllItems(requirment.Reader, requirment.Deleter);

                var item1 = requirment.Helper.MakeValidItem();
                var item2 = requirment.Helper.MakeValidItem();
                var identity = requirment.Helper.MakeInvalidIdentity();
                var response = requirment.Creater.AddSome(new List<TPoco> { item1, item2 }, identity);

                Assert.IsNull(response.Items);
                requirment.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode);
                requirment.Helper.NoItemsExist(requirment.Reader);
            }
        }

        [Test]
        public void DoesNotAddWhenAllItemsDoNotExistAndAreValidAndIdentityIsNull()
        {
            using (var requirment = this.Requirement())
            {
                requirment.Helper.RemoveAllItems(requirment.Reader, requirment.Deleter);

                var item1 = requirment.Helper.MakeValidItem();
                var item2 = requirment.Helper.MakeValidItem();
                var response = requirment.Creater.AddSome(new List<TPoco> { item1, item2 }, null);

                Assert.IsNull(response.Items);
                requirment.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode);
                requirment.Helper.NoItemsExist(requirment.Reader);
            }
        }

        [Test]
        public void DoesNotAddWhenAnyItemAlreadyExistsAndIdentityIsAllowed()
        {
            using (var requirment = this.Requirement())
            {
                requirment.Helper.RemoveAllItems(requirment.Reader, requirment.Deleter);

                var item1 = requirment.Helper.MakeValidItem();
                var item2 = requirment.Helper.MakeValidItem();
                var identity = requirment.Helper.MakeValidIdentity();

                var addedItem1 = requirment.Helper.AddItems(new List<TPoco> { item1 }, requirment.Creater, requirment.Reader).First();
                var response = requirment.Creater.AddSome(new List<TPoco> { addedItem1, item2 }, identity);

                Assert.IsNull(response.Items);
                requirment.Helper.ResponseFailsWithError(response, BusinessCore.AddFailedCode);
                requirment.Helper.ItemCountIs(1, requirment.Reader);
            }
        }

        [Test]
        public void DoesNotAddWhenAnyItemAlreadyExistsAndIdentityIsNotAllowed()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void DoesNotAddWhenAnyItemAlreadyExistslAndIdentityIsNull()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void DoesNotAddWhenAnyItemsAreInvalidAndIdentityIsAllowed()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void DoesNotAddWhenAnyItemsAreInvalidAndIdentityIsNotAllowed()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void DoesNotAddWhenAnyItemsAreInvalidAndIdentityIsNull()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void DoesNotAddWhenAnyItemsAreNullAndIdentityIsAllowed()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void DoesNotAddWhenAnyItemsAreNullAndIdentityIsNotAllowed()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void DoesNotAddWhenAnyItemsAreNullAndIdentityIsNull()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void DoesNotAddWhenItemsIsNullAndIdentityIsAllowed()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void DoesNotAddWhenItemsIsNullAndIdentityIsNotAllowed()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void DoesNotAddWhenItemsIsNullAndIdentityIsNull()
        {
            throw new NotImplementedException();
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
