namespace Trooper.Thorny.Business.TestSuit
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Trooper.Interface.Thorny.Business.Operation.Single;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;
    using Trooper.Interface.Thorny.TestSuit;
    using Security;

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

        public abstract IEnumerable<TPoco> MakeValidItems();

        public abstract IEnumerable<TPoco> MakeInvalidItems();        

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

        public abstract IEnumerable<IIdentity> MakeAllowedIdentities();

        public abstract IEnumerable<IIdentity> MakeDeniedIdentities();

        public virtual IEnumerable<IIdentity> MakeInvalidIdentities()
        {
            return new List<Identity> { null, new Identity() };
        }

        public abstract IIdentity GetAdminIdentity();

        #endregion

        #region crud operations

        public virtual IList<TPoco> AddItems(List<TPoco> validItems)
        {
            var admin = this.GetAdminIdentity();
            var before = this.GetAllItems().Count();
            var response = boCreater.AddSome(validItems, admin);

            Assert.That(this.ResponseIsOk(response));
            Assert.IsNotNull(response.Items);

            var after = this.GetAllItems().Count();

            Assert.That(after == before + validItems.Count());

            return response.Items.ToList();
        }        

        public virtual TPoco AddItem(TPoco validItem)
        {
            var admin = this.GetAdminIdentity();
            var response = boCreater.Add(validItem, admin);

            Assert.That(this.ResponseIsOk(response));
            Assert.IsNotNull(response.Item);
            Assert.That(this.NonIdentifersAreEqual(validItem, response.Item));
            Assert.That(!this.IdentifiersAreEqual(validItem, response.Item));
            Assert.That(this.ItemExists(response.Item));

            return response.Item;
        }
        
        public virtual TPoco GetItem(TPoco exitingItem)
        {
            var admin = this.GetAdminIdentity();
            var response = boReader.GetByKey(exitingItem, admin);

            Assert.That(this.ResponseIsOk(response));
            Assert.IsNotNull(response.Item);
            Assert.That(this.IdentifiersAreEqual(exitingItem, response.Item));

            return response.Item;
        }
                
        public virtual bool ItemExists(TPoco validItem)
        {
            var admin = this.GetAdminIdentity();
            var response = boReader.ExistsByKey(validItem, admin);

            Assert.That(this.ResponseIsOk(response));

            return response.Item;
        }
        
        public virtual void RemoveAllItems()
        {
            var admin = this.GetAdminIdentity();
            var allResponse = boReader.GetAll(admin);

            Assert.That(this.ResponseIsOk(allResponse));
            Assert.IsNotNull(allResponse.Items);

            var deleteResponse = boDeleter.DeleteSomeByKey(allResponse.Items, admin);

            Assert.That(this.ResponseIsOk(deleteResponse));
        }        

        public virtual IList<TPoco> GetAllItems()
        {
            var admin = this.GetAdminIdentity();
            var allResponse = boReader.GetAll(admin);

            Assert.That(this.ResponseIsOk(allResponse));
            Assert.IsNotNull(allResponse.Items);

            return allResponse.Items;
        }        

        public virtual bool ItemCountIs(int count)
        {
            return this.GetAllItems().Count() == count;
        }
        
        public virtual bool StoredItemsAreEqualTo(IList<TPoco> items)
        {
            var storedItems = this.GetAllItems();
            
            if (items.Count != storedItems.Count)
            {
                return false;
            }

            return items.All(i => storedItems.Any(si => this.AreEqual(i, si)));
        }        

        public virtual bool NoItemsExist()
        {
            var admin = this.GetAdminIdentity();
            var response = boReader.GetAll(admin);

            Assert.That(this.ResponseIsOk(response));

            return !response.Items.Any();
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
            foreach (var item1 in this.MakeValidItems())
            {
                var item2 = this.Copy(item1);

                this.ChangeNonIdentifiers(item2);

                Assert.That(this.IdentifiersAreEqual(item1, item2), "The ChangeNonIdentifiers(item) method should not change the identifier properties");
                Assert.That(!this.NonIdentifersAreEqual(item1, item2), "The ChangeNonIdentifiers(item) method should change the none-identifer properties of the item to different values.");
            }
        }

        public virtual void AnItemIsNewAndIdenticalWhenCopied()
        {
            foreach (var item1 in this.MakeValidItems())
            {
                var item2 = this.Copy(item1);

                Assert.That(this.AreEqual(item1, item2), "The Copy method did not correctly copy all the properties of the item.");
                Assert.That(!Object.ReferenceEquals(item1, item2), "The Copy method should return a new instance not the provided instance.");
            }
        }

        public virtual void AnItemIsCopiedAndItsNonIdentifiersChanged()
        {
            foreach (var item1 in this.MakeValidItems())
            {
                var item2 = this.CopyNonIdentifiers(item1);
                this.ChangeNonIdentifiers(item2);

                Assert.That(this.IdentifiersAreEqual(item1, item2), "The ChangeNonIdentifiers(item) should not change the identifier properties");
                Assert.That(!this.NonIdentifersAreEqual(item1, item2), "The ChangeNonIdentifiers(item) should change the none-identifer properties of the item to different values.");
                Assert.That(!Object.ReferenceEquals(item1, item2), "The Copy method should return a new instance not the provided instance.");
            }
        }

        public virtual void InvalidItemsIncludNull()
        {
            var invalidItems = this.MakeInvalidIdentities();

            Assert.That(invalidItems != null);
            Assert.That(invalidItems.Count() > 1);
            Assert.That(invalidItems.Count(i => i == null) > 0);
        }



        #endregion

    }
}
