using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Interface.Thorny.Business.Operation.Single;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;
using Trooper.Interface.Thorny.TestSuit;

namespace Trooper.Thorny.Business.TestSuit
{
    public abstract class TestSuitHelper<TPoco> : ITestSuitHelper<TPoco>
        where TPoco : class
    {
        private int counter = 0;

        protected int IncCounter()
        {
            return this.counter++;
        }        

        public abstract TPoco MakeValidItem();

        public abstract TPoco MakeInvalidItem();

        public abstract TPoco CopyItem(TPoco item);

        public abstract IIdentity MakeValidIdentity();

        public abstract IIdentity MakeInvalidIdentity();

        public IList<TPoco> AddItems(
            List<TPoco> validItems, 
            IIdentity validIdentity, 
            IBusinessCreate<TPoco> boCreater, 
            IBusinessRead<TPoco> boReader)
        {
            var before = this.GetAllItems(validIdentity, boReader).Count();
            var response = boCreater.AddSome(validItems, validIdentity);
            this.ResponseIsOk(response);

            Assert.IsNotNull(response.Items);

            var after = this.GetAllItems(validIdentity, boReader).Count();

            Assert.That(after == before + validItems.Count());

            return response.Items.ToList();
        }

        public IList<TPoco> AddItems(List<TPoco> validItems, IBusinessCreate<TPoco> boCreater, IBusinessRead<TPoco> boReader)
        {
            return this.AddItems(validItems, this.MakeValidIdentity(), boCreater, boReader);
        }

        public TPoco AddItem(TPoco validItem, IIdentity validIdentity, IBusinessCreate<TPoco> boCreater, IBusinessRead<TPoco> boReader)
        {
            var response = boCreater.Add(validItem, validIdentity);
            this.ResponseIsOk(response);

            Assert.IsNotNull(response.Item);
            Assert.That(this.NonIdentifersAreEqual(validItem, response.Item));
            Assert.That(!this.IdentifierAreEqual(validItem, response.Item));
            Assert.That(this.ItemExists(response.Item, boReader));

            return response.Item;
        }

        public TPoco AddItem(TPoco validItem, IBusinessCreate<TPoco> boCreater, IBusinessRead<TPoco> boReader)
        {
            return this.AddItem(validItem, this.MakeValidIdentity(), boCreater, boReader);
        }

        public TPoco GetItem(TPoco exitingItem, IIdentity validIdentity, IBusinessRead<TPoco> boReader)
        {
            var response = boReader.GetByKey(exitingItem, validIdentity);
            this.ResponseIsOk(response);

            Assert.IsNotNull(response.Item);
            Assert.That(this.IdentifierAreEqual(exitingItem, response.Item));

            return response.Item;
        }

        public TPoco GetItem(TPoco existingItem, IBusinessRead<TPoco> boReader)
        {
            return this.GetItem(existingItem, this.MakeValidIdentity(), boReader);
        }

        public bool ItemExists(TPoco validItem, IIdentity validIdentity, IBusinessRead<TPoco> boReader)
        {
            var response = boReader.ExistsByKey(validItem, validIdentity);

            this.ResponseIsOk(response);

            return response.Item;
        }

        public bool ItemExists(TPoco validItem, IBusinessRead<TPoco> boReader)
        {
            return ItemExists(validItem, this.MakeValidIdentity(), boReader);
        }

        public void RemoveAllItems(IIdentity validIdentity, IBusinessRead<TPoco> boReader, IBusinessDelete<TPoco> boDeleter)
        {
            var allResponse = boReader.GetAll(validIdentity);

            this.ResponseIsOk(allResponse);

            Assert.IsNotNull(allResponse.Items);

            var deleteResponse = boDeleter.DeleteSomeByKey(allResponse.Items, validIdentity);

            this.ResponseIsOk(deleteResponse);
        }

        public void RemoveAllItems(IBusinessRead<TPoco> boReader, IBusinessDelete<TPoco> boDeleter)
        {
            this.RemoveAllItems(this.MakeValidIdentity(), boReader, boDeleter);
        }

        public IList<TPoco> GetAllItems(IIdentity validIdentity, IBusinessRead<TPoco> boReader)
        {
            var allResponse = boReader.GetAll(validIdentity);

            this.ResponseIsOk(allResponse);

            Assert.IsNotNull(allResponse.Items);

            return allResponse.Items;
        }

        public IList<TPoco> GetAllItems(IBusinessRead<TPoco> boReader)
        {
            return this.GetAllItems(this.MakeValidIdentity(), boReader);
        }

        public bool ItemCountIs(int count, IIdentity validIdentity, IBusinessRead<TPoco> boReader)
        {
            return this.GetAllItems(validIdentity, boReader).Count() == count;
        }

        public bool ItemCountIs(int count, IBusinessRead<TPoco> boReader)
        {
            return this.ItemCountIs(count, this.MakeValidIdentity(), boReader);
        }

        public void NoItemsExist(IIdentity validIdentity, IBusinessRead<TPoco> boReader)
        {
            var response = boReader.GetAll(validIdentity);
            this.ResponseIsOk(response);
            Assert.That(!response.Items.Any());
        }

        public void NoItemsExist(IBusinessRead<TPoco> boReader)
        {
            this.NoItemsExist(this.MakeValidIdentity(), boReader);
        }

        public bool AreEqual(TPoco itemA, TPoco itemB)
        {
            return this.IdentifierAreEqual(itemA, itemB) && this.NonIdentifersAreEqual(itemA, itemB);            
        }

        public abstract bool IdentifierAreEqual(TPoco itemA, TPoco itemB);

        public abstract bool NonIdentifersAreEqual(TPoco itemA, TPoco itemB);

        public abstract void ChangeNonIdentifiers(TPoco item);

        public void ResponseIsOk(IResponse response)
        {
            Assert.IsNotNull(response, "The response is null");

            if (!response.Ok)
            {
                Assert.IsNotNull(response.Messages, "The response is not ok and there are no messages why.");

                var messages = response.Messages.Select(m => string.Format("[Code: {0}] [Level: {1}] [Content: {2}]", m.Code, m.Level, m.Content));

                Assert.Fail("The response is not ok.\n" + string.Join(Environment.NewLine, messages));
            }
        }

        public void ResponseFailsWithError(IResponse response, string code)
        {
            Assert.IsNotNull(response, "The response is null");
            Assert.IsFalse(response.Ok);
            Assert.IsNotNull(response.Messages);
            Assert.That(response.Messages.Any(m => m.Code == code && m.Level == MessageAlertLevel.Error));
        }

        public void SelfTestHelper()
        {
            this.NewValidItemsAreDifferent();
            this.NewValidItemsIncrementCounter();
            this.NonIdentifiersAreDifferentWhenChanged();
            this.AnItemIsNewAndIdenticalWhenCopied();
        }

        public virtual void NewValidItemsAreDifferent()
        {
            var item1 = this.MakeInvalidItem();
            var item2 = this.MakeInvalidItem();

            Assert.That(!this.NonIdentifersAreEqual(item1, item2),
                "When making new valid items the none-identifying properties should be different. Consider 'Counter' generate unique properties");
        }

        public virtual void NewValidItemsIncrementCounter()
        {
            var item1 = this.MakeInvalidItem();
            var c1 = this.counter;
            var item2 = this.MakeInvalidItem();
            var c2 = this.counter;

            Assert.That(c1 != c2, "The implementation of MakeInvalidItem() is not calling IncCounter()");
        }

        public virtual void NonIdentifiersAreDifferentWhenChanged()
        {
            var item1 = this.MakeValidItem();
            var item2 = this.CopyItem(item1);

            this.ChangeNonIdentifiers(item2);

            Assert.That(this.IdentifierAreEqual(item1, item2), "The ChangeNonIdentifiers(item) should not change the identifier properties");
            Assert.That(!this.NonIdentifersAreEqual(item1, item2), "The ChangeNonIdentifiers(item) should change the none-identifer properties of the item to different values.");
        }

        public virtual void AnItemIsNewAndIdenticalWhenCopied()
        {
            var item1 = this.MakeValidItem();
            var item2 = this.CopyItem(item1);

            Assert.That(this.AreEqual(item1, item2), "The CopyItem method did not correctly copy all the properties of the item.");
            Assert.That(!Object.ReferenceEquals(item1, item2), "The CopyItem should return a new instance not the provided instance.");
        }
    }
}
