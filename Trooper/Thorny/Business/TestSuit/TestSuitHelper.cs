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

        private IBusinessCreate<TPoco> boCreater;
        private IBusinessRead<TPoco> boReader;
        private IBusinessDelete<TPoco> boDeleter;

        public TestSuitHelper(IBusinessCreate<TPoco> boCreater, IBusinessRead<TPoco> boReader, IBusinessDelete<TPoco> boDeleter)
        {
            this.boCreater = boCreater;
            this.boReader = boReader;
            this.boDeleter = boDeleter;
        }

        protected int IncCounter()
        {
            return this.counter++;
        }

        public abstract TPoco MakeValidItem();

        public abstract TPoco MakeInvalidItem();

        public abstract TPoco CopyItem(TPoco item);

        public abstract IIdentity MakeValidIdentity();

        public abstract IIdentity MakeInvalidIdentity();

        public virtual IList<TPoco> AddItems(List<TPoco> validItems, IIdentity validIdentity)
        {
            var before = this.GetAllItems(validIdentity).Count();
            var response = boCreater.AddSome(validItems, validIdentity);

            Assert.That(this.ResponseIsOk(response));
            Assert.IsNotNull(response.Items);

            var after = this.GetAllItems(validIdentity).Count();

            Assert.That(after == before + validItems.Count());

            return response.Items.ToList();
        }

        public IList<TPoco> AddItems(List<TPoco> validItems)
        {
            return this.AddItems(validItems, this.MakeValidIdentity());
        }

        public virtual TPoco AddItem(TPoco validItem, IIdentity validIdentity)
        {
            var response = boCreater.Add(validItem, validIdentity);

            Assert.That(this.ResponseIsOk(response));
            Assert.IsNotNull(response.Item);
            Assert.That(this.NonIdentifersAreEqual(validItem, response.Item));
            Assert.That(!this.IdentifiersAreEqual(validItem, response.Item));
            Assert.That(this.ItemExists(response.Item));

            return response.Item;
        }

        public TPoco AddItem(TPoco validItem)
        {
            return this.AddItem(validItem, this.MakeValidIdentity());
        }

        public virtual TPoco GetItem(TPoco exitingItem, IIdentity validIdentity)
        {
            var response = boReader.GetByKey(exitingItem, validIdentity);
            Assert.That(this.ResponseIsOk(response));
            Assert.IsNotNull(response.Item);
            Assert.That(this.IdentifiersAreEqual(exitingItem, response.Item));

            return response.Item;
        }

        public TPoco GetItem(TPoco existingItem)
        {
            return this.GetItem(existingItem, this.MakeValidIdentity());
        }

        public virtual bool ItemExists(TPoco validItem, IIdentity validIdentity)
        {
            var response = boReader.ExistsByKey(validItem, validIdentity);

            Assert.That(this.ResponseIsOk(response));

            return response.Item;
        }

        public bool ItemExists(TPoco validItem)
        {
            return ItemExists(validItem, this.MakeValidIdentity());
        }

        public virtual void RemoveAllItems(IIdentity validIdentity)
        {
            var allResponse = boReader.GetAll(validIdentity);

            Assert.That(this.ResponseIsOk(allResponse));
            Assert.IsNotNull(allResponse.Items);

            var deleteResponse = boDeleter.DeleteSomeByKey(allResponse.Items, validIdentity);

            Assert.That(this.ResponseIsOk(deleteResponse));
        }

        public void RemoveAllItems()
        {
            this.RemoveAllItems(this.MakeValidIdentity());
        }

        public virtual IList<TPoco> GetAllItems(IIdentity validIdentity)
        {
            var allResponse = boReader.GetAll(validIdentity);

            Assert.That(this.ResponseIsOk(allResponse));
            Assert.IsNotNull(allResponse.Items);

            return allResponse.Items;
        }

        public IList<TPoco> GetAllItems()
        {
            return this.GetAllItems(this.MakeValidIdentity());
        }

        public virtual bool ItemCountIs(int count, IIdentity validIdentity)
        {
            return this.GetAllItems(validIdentity).Count() == count;
        }

        public bool ItemCountIs(int count)
        {
            return this.ItemCountIs(count, this.MakeValidIdentity());
        }

        public virtual bool StoredItemsAreEqualTo(IList<TPoco> items, IIdentity validIdentity)
        {
            var storedItems = this.GetAllItems(validIdentity);
            
            if (items.Count != storedItems.Count)
            {
                return false;
            }

            return items.All(i => storedItems.Any(si => this.AreEqual(i, si)));
        }

        public bool StoredItemsAreEqualTo(IList<TPoco> items)
        {
            return this.StoredItemsAreEqualTo(items, this.MakeValidIdentity());
        }

        public virtual bool NoItemsExist(IIdentity validIdentity)
        {
            var response = boReader.GetAll(validIdentity);

            Assert.That(this.ResponseIsOk(response));

            return !response.Items.Any();
        }

        public bool NoItemsExist()
        {
            return this.NoItemsExist(this.MakeValidIdentity());
        }

        public virtual bool AreEqual(TPoco itemA, TPoco itemB)
        {
            return this.IdentifiersAreEqual(itemA, itemB) && this.NonIdentifersAreEqual(itemA, itemB);            
        }

        public abstract bool IdentifiersAreEqual(TPoco itemA, TPoco itemB);

        public abstract bool NonIdentifersAreEqual(TPoco itemA, TPoco itemB);

        public abstract void ChangeNonIdentifiers(TPoco item);

        public TPoco CopyAndChangeItemNonIdentifiers(TPoco item)
        {
            var copy = this.CopyItem(item);
            this.ChangeNonIdentifiers(item);

            return copy;
        }

        public bool ResponseIsOk(IResponse response)
        {
            if (response == null)
            {
                return false;
            }

            return response.Ok;            
        }

        public string ResponseNotOkMessages(IResponse response)
        {
            if (response == null)
            {
                return "The response is null";
            }

            if (response.Ok)
            {
                return string.Empty;
            }

            if (response.Messages == null)
            {
                return "The response is not ok and there are no messages why.";
            }

            var messages = response.Messages.Select(m => string.Format("[Code: {0}] [Level: {1}] [Content: {2}]", m.Code, m.Level, m.Content));

            return "The response is not ok.\n" + string.Join(Environment.NewLine, messages);

        }

        public bool ResponseFailsWithError(IResponse response, string code)
        {
            return response != null 
                && !response.Ok 
                && response.Messages.Any(m => m.Code == code && m.Level == MessageAlertLevel.Error);
        }

        public void SelfTestHelper()
        {           
            this.NonIdentifiersAreDifferentWhenChanged();
            this.AnItemIsNewAndIdenticalWhenCopied();
            this.AnItemIsCopiedAndItsNonIdentifiersChanged();
        }

        public virtual void NonIdentifiersAreDifferentWhenChanged()
        {
            var item1 = this.MakeValidItem();
            var item2 = this.CopyItem(item1);

            this.ChangeNonIdentifiers(item2);

            Assert.That(this.IdentifiersAreEqual(item1, item2), "The ChangeNonIdentifiers(item) should not change the identifier properties");
            Assert.That(!this.NonIdentifersAreEqual(item1, item2), "The ChangeNonIdentifiers(item) should change the none-identifer properties of the item to different values.");
        }

        public virtual void AnItemIsNewAndIdenticalWhenCopied()
        {
            var item1 = this.MakeValidItem();
            var item2 = this.CopyItem(item1);

            Assert.That(this.AreEqual(item1, item2), "The CopyItem method did not correctly copy all the properties of the item.");
            Assert.That(!Object.ReferenceEquals(item1, item2), "The CopyItem should return a new instance not the provided instance.");
        }               

        public virtual void AnItemIsCopiedAndItsNonIdentifiersChanged()
        {
            var item1 = this.MakeInvalidItem();
            var item2 = this.CopyAndChangeItemNonIdentifiers(item1);

            Assert.That(this.IdentifiersAreEqual(item1, item2), "The ChangeNonIdentifiers(item) should not change the identifier properties");
            Assert.That(!this.NonIdentifersAreEqual(item1, item2), "The ChangeNonIdentifiers(item) should change the none-identifer properties of the item to different values.");
            Assert.That(!Object.ReferenceEquals(item1, item2), "The CopyItem should return a new instance not the provided instance.");
        }

    }
}
