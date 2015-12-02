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

    public abstract class TestSuitHelper<TPoco>
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

        public virtual int DefaultRequiredValidItems
        {
            get
            {
                return 2;
            }
        }

        public virtual int DefaultRequiredInvalidItems
        {
            get
            {
                return 2;
            }
        }

        public int DefaultRequiredItems
        {
            get
            {
                if (this.DefaultRequiredInvalidItems >= this.DefaultRequiredValidItems)
                {
                    return this.DefaultRequiredInvalidItems;
                }

                return this.DefaultRequiredValidItems;
            }
        }

        #region item manipulation

        #region valid item generation

        public IEnumerable<TPoco> MakeValidItems()
        {
            return this.MakeValidItems(this.DefaultRequiredValidItems);
        }

        public abstract IEnumerable<TPoco> MakeValidItems(int required);

        #endregion

        #region invalid item generation

        public IEnumerable<TPoco> MakeInvalidItems()
        {
            return this.MakeInvalidItems(this.DefaultRequiredInvalidItems, true);
        }

        public IEnumerable<TPoco> MakeInvalidItems(bool incNull)
        {
            return this.MakeInvalidItems(this.DefaultRequiredInvalidItems, incNull);
        }

        public IEnumerable<TPoco> MakeInvalidItems(int required, bool incNull)
        {
            var items = this.MakeValidItems(required - (incNull ? 1 : 0)).ToList();

            this.MakeInvalidItems(items);            

            if (!incNull)
            {
                return items;
            }

            var withNull = items.ToList();
            withNull.Add(null);

            return withNull;
        }

        public void MakeInvalidItem(TPoco validItem)
        {
            this.MakeInvalidItems(new List<TPoco> { validItem });
        }

        public abstract void MakeInvalidItems(List<TPoco> validItems);

        #endregion

        /// <summary>
        ///     Idenitifers and non-identifiers should be equal
        /// </summary>
        /// <param name="itemA"></param>
        /// <param name="itemB"></param>
        /// <returns></returns>
        public virtual bool AreEqual(TPoco itemA, TPoco itemB)
        {
            return this.IdentifiersAreEqual(itemA, itemB) && this.NonIdentifiersAreEqual(itemA, itemB);
        }

        public bool AreEqual(IEnumerable<TPoco> itemsA, IEnumerable<TPoco> itemsB)
        {
            if (itemsA == null)
            {
                throw new ArgumentNullException(nameof(itemsA));
            }

            if (itemsB == null)
            {
                throw new ArgumentNullException(nameof(itemsB));
            }

            if (itemsA.Count() != itemsB.Count())
            {
                return false;
            }

            return itemsA.All(a => itemsB.Any(b => this.AreEqual(a, b)))
                && itemsB.All(b => itemsA.Any(a => this.AreEqual(a, b)));
        }

        public bool Contains(IEnumerable<TPoco> superSet, IEnumerable<TPoco> subSet)
        {
            if (superSet == null)
            {
                throw new ArgumentNullException(nameof(superSet));
            }

            if (subSet == null)
            {
                throw new ArgumentNullException(nameof(subSet));
            }

            if (subSet.Count() > subSet.Count())
            {
                return false;
            }

            return subSet.All(a => superSet.Any(b => this.AreEqual(a, b)));
        }

        public abstract bool IdentifiersAreEqual(TPoco itemA, TPoco itemB);

        public bool IdentifiersAreEqual(IEnumerable<TPoco> itemsA, IEnumerable<TPoco> itemsB)
        {
            if (itemsA == null)
            {
                throw new ArgumentNullException(nameof(itemsA));
            }

            if (itemsB == null)
            {
                throw new ArgumentNullException(nameof(itemsB));
            }

            if (itemsA.Count() != itemsB.Count())
            {
                return false;
            }

            return itemsA.All(a => itemsB.Any(b => this.IdentifiersAreEqual(a, b)))
                && itemsB.All(b => itemsA.Any(a => this.IdentifiersAreEqual(a, b)));
        }

        public bool ContainsIdentifiers(IEnumerable<TPoco> superSet, IEnumerable<TPoco> subSet)
        {
            if (superSet == null)
            {
                throw new ArgumentNullException(nameof(superSet));
            }

            if (subSet == null)
            {
                throw new ArgumentNullException(nameof(subSet));
            }

            if (subSet.Count() > subSet.Count())
            {
                return false;
            }

            return subSet.All(a => superSet.Any(b => this.IdentifiersAreEqual(a, b)));
        }

        public abstract bool NonIdentifiersAreEqual(TPoco itemA, TPoco itemB);

        public bool NonIdentifiersAreEqual(IEnumerable<TPoco> itemsA, IEnumerable<TPoco> itemsB)
        {
            if (itemsA == null)
            {
                throw new ArgumentNullException(nameof(itemsA));
            }

            if (itemsB == null)
            {
                throw new ArgumentNullException(nameof(itemsB));
            }

            if (itemsA.Count() != itemsB.Count())
            {
                return false;
            }

            return itemsA.All(a => itemsB.Any(b => this.NonIdentifiersAreEqual(a, b))) 
                && itemsB.All(b => itemsA.Any(a => this.NonIdentifiersAreEqual(a, b)));
        }

        public bool ContainsNonIdentifiers(IEnumerable<TPoco> superSet, IEnumerable<TPoco> subSet)
        {
            if (superSet == null)
            {
                throw new ArgumentNullException(nameof(superSet));
            }

            if (subSet == null)
            {
                throw new ArgumentNullException(nameof(subSet));
            }

            if (subSet.Count() > subSet.Count())
            {
                return false;
            }

            return subSet.All(a => superSet.Any(b => this.NonIdentifiersAreEqual(a, b)));
        }

        public TPoco Copy(TPoco item)
        {
            var result = new TPoco();

            this.CopyIdentifiers(item, result);
            this.CopyNonIdentifiers(item, result);

            return result;
        }

        public IEnumerable<TPoco> Copy(IEnumerable<TPoco> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items)
            {
                yield return this.Copy(item);
            }
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

        /// <summary>
        ///     item must be changed so that its non-identifers fields do not match any of the same fields on otherItems
        /// </summary>
        /// <param name="item"></param>
        /// <param name="otherItems"></param>
        public abstract void ChangeNonIdentifiers(TPoco item, IEnumerable<TPoco> otherItems);

        public void ChangeNonIdentifiers(TPoco item, IEnumerable<TPoco> otherItems, bool filterIdentityMatch)
        {
            this.ChangeNonIdentifiers(item, filterIdentityMatch ? otherItems.Where(i => this.IdentifiersAreEqual(i, item)) : otherItems);
        }

        public void ChangeNonIdentifiers(IEnumerable<TPoco> items, IEnumerable<TPoco> otherItems)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (otherItems == null) throw new ArgumentNullException(nameof(otherItems));

            if (items.Any(i => otherItems.Any(oi => ReferenceEquals(oi, oi))))
            {
                throw new Exception($"No element in {nameof(items)} can reference anything in {nameof(otherItems)}");
            }
        }

        public string ChangeProperty(string currentValue, IEnumerable<string> otherValues, string pattern)
        {
            var i = 1;
            var newValue = string.Format(pattern, i);

            while (currentValue == newValue || otherValues.Any(value => value == newValue))
            {
                i++;
                newValue = string.Format(pattern, i);
            }

            return newValue;
        }

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

        public IList<TPoco> AddValidItems()
        {
            return this.AddValidItems(this.DefaultRequiredValidItems);
        }

        public IList<TPoco> AddValidItems(int required)
        {
            var validItems = this.MakeValidItems(required);

            return this.AddItems(validItems.ToList());
        }

        public virtual TPoco AddItem(TPoco validItem)
        {
            var admin = this.GetAdminIdentity();
            var response = boCreater.Add(validItem, admin);

            Assert.That(this.ResponseIsOk(response));
            Assert.IsNotNull(response.Item);
            Assert.That(this.NonIdentifiersAreEqual(validItem, response.Item));
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

        public bool ItemCountIs(int count)
        {
            return this.GetAllItems().Count() == count;
        }

        public bool HasNoItems()
        {
            return this.ItemCountIs(0);
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

            this.ValidItemsAreOk();
            this.InvalidItemsAreOk();
            this.AllowedIdentitiesAreOk();
            this.DeniedIdentitiesAreOk();
            this.InvalidIdentitiesAreOk();
        }

        public void NonIdentifiersAreDifferentWhenChanged()
        {
            var validItems = this.MakeValidItems().ToList();

            for (var t = 0; t<validItems.Count(); t++)
            {
                var others = new List<TPoco>();
                var target = validItems[t];
                var original = this.Copy(validItems[t]);


                for (var i = 0; i<validItems.Count(); i++)
                {
                    if (i != t)
                    {
                        others.Add(validItems[i]);
                    }
                }
                
                this.ChangeNonIdentifiers(target, others);

                Assert.That(this.IdentifiersAreEqual(target, original), $"The {nameof(this.ChangeNonIdentifiers)} method should not change the identifier properties");
                Assert.That(!this.NonIdentifiersAreEqual(target, original), $"The {nameof(this.ChangeNonIdentifiers)} method should change the none-identifer properties of the item to different values.");

                foreach (var item in others)
                {
                    Assert.That(this.AreEqual(target, item), Is.Not.True, "The target item should not be Equal to any other item");
                }        
            }
        }
                
        public void AnItemIsNewAndIdenticalWhenCopied()
        {
            foreach (var item1 in this.MakeValidItems())
            {
                var item2 = this.Copy(item1);

                Assert.That(this.AreEqual(item1, item2), "The Copy method did not correctly copy all the properties of the item.");
                Assert.That(!Object.ReferenceEquals(item1, item2), "The Copy method should return a new instance not the provided instance.");
            }
        }

        public void AnItemIsCopiedAndItsNonIdentifiersChanged()
        {
            var validItems = this.MakeValidItems();

            foreach (var item1 in validItems)
            {
                var item2 = this.CopyNonIdentifiers(item1);
                this.ChangeNonIdentifiers(item2, validItems.Where(i => !this.AreEqual(i, item2)));

                Assert.That(this.IdentifiersAreEqual(item1, item2), "The ChangeNonIdentifiers(item) should not change the identifier properties");
                Assert.That(!this.NonIdentifiersAreEqual(item1, item2), "The ChangeNonIdentifiers(item) should change the none-identifer properties of the item to different values.");
                Assert.That(!Object.ReferenceEquals(item1, item2), "The Copy method should return a new instance not the provided instance.");
            }
        }

        public void ValidItemsAreOk()
        {
            var validItems = this.MakeValidItems();

            Assert.That(validItems, Is.Not.Null);
            Assert.That(validItems.Count(), Is.GreaterThan(1));
            Assert.That(validItems.Count(i => i == null), Is.EqualTo(0));
        }

        public void InvalidItemsAreOk()
        {
            var invalidItems = this.MakeInvalidItems();

            Assert.That(invalidItems, Is.Not.Null);
            Assert.That(invalidItems.Count(), Is.GreaterThan(1));
            Assert.That(invalidItems.Count(i => i == null), Is.EqualTo(1));
        }

        public void AllowedIdentitiesAreOk()
        {
            var allowedIdentities = this.MakeAllowedIdentities();

            Assert.That(allowedIdentities, Is.Not.Null);
            Assert.That(allowedIdentities.Count(), Is.GreaterThan(0));
            Assert.That(allowedIdentities.Count(i => i == null), Is.EqualTo(0));
        }

        public void DeniedIdentitiesAreOk()
        {
            var deniedIdentities = this.MakeDeniedIdentities();

            Assert.That(deniedIdentities, Is.Not.Null);
            Assert.That(deniedIdentities.Count(), Is.GreaterThan(0));
            Assert.That(deniedIdentities.Count(i => i == null), Is.EqualTo(0));
        }

        public void InvalidIdentitiesAreOk()
        {
            var invalidIdentities = this.MakeInvalidIdentities();

            Assert.That(invalidIdentities, Is.Not.Null);
            Assert.That(invalidIdentities.Count(), Is.GreaterThan(1));
            Assert.That(invalidIdentities.Count(i => i == null), Is.EqualTo(1));
        }


        #endregion

    }
}
