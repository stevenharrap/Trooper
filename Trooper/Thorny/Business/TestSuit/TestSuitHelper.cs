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
using Trooper.Thorny.Business.Security;

namespace Trooper.Thorny.Business.TestSuit
{
    public abstract class TestSuitHelper<TPoco> : ITestSuitHelper<TPoco>
        where TPoco : class, new()
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

        #region item manipulation

        public abstract TPoco MakeValidItem();

        public abstract TPoco MakeInvalidItem();        

        public virtual bool AreEqual(TPoco itemA, TPoco itemB)
        {
            return this.IdentifiersAreEqual(itemA, itemB) && this.NonIdentifersAreEqual(itemA, itemB);
        }

        public abstract bool IdentifiersAreEqual(TPoco itemA, TPoco itemB);

        public abstract bool NonIdentifersAreEqual(TPoco itemA, TPoco itemB);

        public TPoco Copy(TPoco item)
        {
            var result = new TPoco();

            this.CopyIdentifiers(item, result);
            this.CopyNonIdentifiers(item, result);

            return result;
        }

        public void Copy(TPoco source, TPoco destination)
        {
            this.CopyIdentifiers(source, destination);
            this.CopyNonIdentifiers(source, destination);
        }

        public TPoco CopyIdentifiers(TPoco item)
        {
            var result = new TPoco();

            this.CopyIdentifiers(item, result);

            return result;
        }
        
        public abstract void CopyIdentifiers(TPoco source, TPoco destination);

        public TPoco CopyNonIdentifiers(TPoco item)
        {
            var result = new TPoco();

            this.CopyIdentifiers(item, result);

            return result;
        }

        public abstract void ChangeNonIdentifiers(TPoco item);

        public abstract void CopyNonIdentifiers(TPoco source, TPoco destination);

        #endregion

        #region identity manipulation

        public abstract IIdentity MakeAllowedIdentity();

        public abstract IIdentity MakeDeniedIdentity();

        public virtual IIdentity MakeInvalidIdentity()
        {
            return new Identity();
        }

        #endregion

        #region crud operations

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
            return this.AddItems(validItems, this.MakeAllowedIdentity());
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
            return this.AddItem(validItem, this.MakeAllowedIdentity());
        }

        public TPoco AddItem(IIdentity validIdentity)
        {
            var response = boCreater.Add(this.MakeValidItem(), validIdentity);

            return response.Item;
        }

        public TPoco AddItem()
        {
            return this.AddItem(this.MakeAllowedIdentity());
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
            return this.GetItem(existingItem, this.MakeAllowedIdentity());
        }

        public virtual bool ItemExists(TPoco validItem, IIdentity validIdentity)
        {
            var response = boReader.ExistsByKey(validItem, validIdentity);

            Assert.That(this.ResponseIsOk(response));

            return response.Item;
        }

        public bool ItemExists(TPoco validItem)
        {
            return ItemExists(validItem, this.MakeAllowedIdentity());
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
            this.RemoveAllItems(this.MakeAllowedIdentity());
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
            return this.GetAllItems(this.MakeAllowedIdentity());
        }

        public virtual bool ItemCountIs(int count, IIdentity validIdentity)
        {
            return this.GetAllItems(validIdentity).Count() == count;
        }

        public bool ItemCountIs(int count)
        {
            return this.ItemCountIs(count, this.MakeAllowedIdentity());
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
            return this.StoredItemsAreEqualTo(items, this.MakeAllowedIdentity());
        }

        public virtual bool NoItemsExist(IIdentity validIdentity)
        {
            var response = boReader.GetAll(validIdentity);

            Assert.That(this.ResponseIsOk(response));

            return !response.Items.Any();
        }

        public bool NoItemsExist()
        {
            return this.NoItemsExist(this.MakeAllowedIdentity());
        }

        #endregion

        #region response investigatoin

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

        #endregion

        #region self testing

        public void SelfTestHelper()
        {           
            this.NonIdentifiersAreDifferentWhenChanged();
            this.AnItemIsNewAndIdenticalWhenCopied();
            this.AnItemIsCopiedAndItsNonIdentifiersChanged();
        }        

        public virtual void NonIdentifiersAreDifferentWhenChanged()
        {
            var item1 = this.MakeValidItem();
            var item2 = this.Copy(item1);

            this.ChangeNonIdentifiers(item2);

            Assert.That(this.IdentifiersAreEqual(item1, item2), "The ChangeNonIdentifiers(item) method should not change the identifier properties");
            Assert.That(!this.NonIdentifersAreEqual(item1, item2), "The ChangeNonIdentifiers(item) method should change the none-identifer properties of the item to different values.");
        }

        public virtual void AnItemIsNewAndIdenticalWhenCopied()
        {
            var item1 = this.MakeValidItem();
            var item2 = this.Copy(item1);

            Assert.That(this.AreEqual(item1, item2), "The Copy method did not correctly copy all the properties of the item.");
            Assert.That(!Object.ReferenceEquals(item1, item2), "The Copy method should return a new instance not the provided instance.");
        }               

        public virtual void AnItemIsCopiedAndItsNonIdentifiersChanged()
        {
            var item1 = this.MakeValidItem();
            var item2 = this.CopyNonIdentifiers(item1);
            this.ChangeNonIdentifiers(item2);

            Assert.That(this.IdentifiersAreEqual(item1, item2), "The ChangeNonIdentifiers(item) should not change the identifier properties");
            Assert.That(!this.NonIdentifersAreEqual(item1, item2), "The ChangeNonIdentifiers(item) should change the none-identifer properties of the item to different values.");
            Assert.That(!Object.ReferenceEquals(item1, item2), "The Copy method should return a new instance not the provided instance.");
        }

        #endregion

    }
}
