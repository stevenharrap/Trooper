namespace Trooper.Thorny.Business.TestSuit
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Trooper.Interface.Thorny.Business.Operation.Single;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;
    using Response;
    using Security;
    using Operation.Core;
    using Utility;

    public abstract class TestSuitHelper<TPoco> : TestSuitHelper
        where TPoco : class, new()
    {
        private int counter = 0;
        private int? defaultRequiredValidItems = null;

        private IBusinessCreate<TPoco> boCreater;
        private IBusinessRead<TPoco> boReader;
        private IBusinessDelete<TPoco> boDeleter;

        public TestSuitHelper(IBusinessCreate<TPoco> boCreater, IBusinessRead<TPoco> boReader, IBusinessDelete<TPoco> boDeleter)
        {
            this.boCreater = boCreater;
            this.boReader = boReader;
            this.boDeleter = boDeleter;
        }        

        #region properties

        protected int IncCounter()
        {
            return this.counter++;
        }
        
        /// <summary>
        /// [Tested]
        /// </summary>
        public int DefaultRequiredValidItems
        {
            get
            {
                if (this.defaultRequiredValidItems == null)
                {
                    this.defaultRequiredValidItems = this.MakeValidItems(Keys.Default).Count();
                }

                return (int)this.defaultRequiredValidItems;
            }
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public abstract int DefaultRequiredInvalidItems
        {
            get;
        }
        
        /// <summary>
        /// [Tested]
        /// </summary>
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

        #endregion

        #region identity and none identity property value setting and changing 

        /// <summary>
        /// [Tested]
        /// </summary>
        public string ChangeProperty(string currentValue, IEnumerable<string> otherValues, string pattern)
        {
            if (otherValues == null) throw new ArgumentNullException(nameof(otherValues));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            var i = 1;
            var newValue = string.Format(pattern, i);

            while (currentValue == newValue || otherValues.Any(value => value == newValue))
            {
                i++;
                newValue = string.Format(pattern, i);
            }

            return newValue;
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public long ChangeProperty(int currentValue, IEnumerable<int> otherValues)
        {
            if (otherValues == null) throw new ArgumentNullException(nameof(otherValues));
            
            var newValue = currentValue + 1;

            while (otherValues.Any(v => v == newValue))
            {
                newValue++;
            }

            return newValue;
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public string ChangeProperty(IEnumerable<string> otherValues, string pattern)
        {
            if (otherValues == null) throw new ArgumentNullException(nameof(otherValues));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));

            var i = 1;
            var result = string.Format(pattern, i);

            while (otherValues.Any(value => value == result))
            {
                i++;
                result = string.Format(pattern, i);
            }

            return result;
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public long ChangeProperty(IEnumerable<int> otherValues)
        {
            if (otherValues == null) throw new ArgumentNullException(nameof(otherValues));

            var newValue = 1;

            while (otherValues.Any(v => v == newValue))
            {
                newValue++;
            }

            return newValue;
        }

        public int MakeKeyValue(Keys keysBehaviours, bool keyIsAuto, IEnumerable<int?> otherValues)
        {
            if (otherValues == null) throw new ArgumentNullException(nameof(otherValues));            

            if (keysBehaviours == Keys.Gen || (keysBehaviours == Keys.GenIfMnl && !keyIsAuto))
            {
                var newValue = 1;

                while (otherValues.Any(v => v == newValue))
                {
                    newValue++;
                }

                return newValue;
            }

            return 0;
        }

        #endregion

        #region valid item generation

        /// <summary>
        /// [Tested]
        /// </summary>
        public IEnumerable<TPoco> MakeValidItems(Keys keyBehavious)
        {
            return this.MakeValidItems(null, keyBehavious, new List<TPoco>());
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public IEnumerable<TPoco> MakeValidItems(int? required, Keys keyBehavious)
        {
            return this.MakeValidItems(required, keyBehavious, new List<TPoco>());
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public IEnumerable<TPoco> MakeValidItems(Keys keyBehavious, IEnumerable<TPoco> otherItems)
        {
            return this.MakeValidItems(this.DefaultRequiredValidItems, keyBehavious, otherItems);
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public abstract IEnumerable<TPoco> MakeValidItems(int? required, Keys keyBehavious, IEnumerable<TPoco> otherItems);

        #endregion

        #region invalid item generation                           

        /// <summary>
        /// [Tested]
        /// </summary>
        public void MakeInvalidItem(TPoco item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            this.MakeInvalidItems(false, new List<TPoco> { item }, new List<TPoco>());
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public IEnumerable<TPoco> MakeInvalidItems(bool incNull, int required, Keys keyBehavious)
        {
            var items = this.MakeValidItems(required, keyBehavious).ToList();

            this.MakeInvalidItems(incNull, items, new List<TPoco>());

            return items;
        }

        public IEnumerable<TPoco> MakeInvalidItems(bool incNull, Keys keyBehavious)
        {
            var items = this.MakeValidItems(this.DefaultRequiredInvalidItems, keyBehavious).ToList();

            this.MakeInvalidItems(incNull, items, new List<TPoco>());

            return items;
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public IEnumerable<TPoco> MakeInvalidItems(Keys keyBehavious)
        {
            var items = this.MakeValidItems(this.DefaultRequiredInvalidItems, keyBehavious).ToList();

            this.MakeInvalidItems(items);

            return items;
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public IEnumerable<TPoco> MakeInvalidItems(Keys keyBehavious, IEnumerable<TPoco> otherItems)
        {
            var items = this.MakeValidItems(this.DefaultRequiredInvalidItems, keyBehavious, otherItems).ToList();

            this.MakeInvalidItems(items, otherItems);

            return items;
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public void MakeInvalidItems(List<TPoco> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            MakeInvalidItems(false, items, new List<TPoco>());
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public void MakeInvalidItems(bool incNull, List<TPoco> items, IEnumerable<TPoco> otherItems)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (otherItems == null) throw new ArgumentNullException(nameof(otherItems));

            MakeInvalidItems(items, new List<TPoco>());

            if (incNull)
            {
                items.Add(null);
            }
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public abstract void MakeInvalidItems(List<TPoco> items, IEnumerable<TPoco> otherItems);

        #endregion

        #region item identifier and non-identifier equality

        /// <summary>
        ///     Idenitifers and non-identifiers should be equal
        ///     [Tested]
        /// </summary>
        /// <param name="itemA"></param>
        /// <param name="itemB"></param>
        /// <returns></returns>
        public bool AreEqual(TPoco itemA, TPoco itemB)
        {
            if (itemA == null)
            {
                throw new ArgumentNullException(nameof(itemA));
            }

            if (itemB == null)
            {
                throw new ArgumentNullException(nameof(itemB));
            }

            return this.IdentifiersAreEqual(itemA, itemB) && this.NonIdentifiersAreEqual(itemA, itemB);
        }

        /// <summary>
        /// [Tested]
        /// </summary>
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

        /// <summary>
        /// [Tested]
        /// </summary>
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
        
        /// <summary>
        /// [Tested]
        /// </summary>
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

        /// <summary>
        /// [Tested]
        /// </summary>
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

        /// <summary>
        /// [Tested]
        /// </summary>
        public abstract bool IdentifiersAreEqual(TPoco itemA, TPoco itemB);

        /// <summary>
        /// [Tested]
        /// </summary>
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

        /// <summary>
        /// [Tested]
        /// </summary>
        public abstract bool NonIdentifiersAreEqual(TPoco itemA, TPoco itemB);

        /// <summary>
        /// [Tested]
        /// </summary>  
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

        #endregion

        #region item duplication

        /// <summary>
        /// [Tested]
        /// </summary>  
        public TPoco Copy(TPoco item)
        {
            if (item == null) return null;

            var result = new TPoco();

            this.CopyIdentifiers(item, result);
            this.CopyNonIdentifiers(item, result);

            return result;
        }

        /// <summary>
        /// [Tested]
        /// </summary>  
        public List<TPoco> Copy(IEnumerable<TPoco> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            return items.Select(item => this.Copy(item)).ToList();
        }

        /// <summary>
        /// [Tested]
        /// </summary>  
        public void Copy(TPoco source, TPoco destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            this.CopyIdentifiers(source, destination);
            this.CopyNonIdentifiers(source, destination);
        }

        /// <summary>
        /// [Tested]
        /// </summary>  
        public TPoco CopyIdentifiers(TPoco item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var result = new TPoco();

            this.CopyIdentifiers(item, result);

            return result;
        }

        /// <summary>
        /// [Tested]
        /// </summary>  
        public abstract void CopyIdentifiers(TPoco source, TPoco destination);

        /// <summary>
        /// [Tested]
        /// </summary> 
        public TPoco CopyNonIdentifiers(TPoco item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var result = new TPoco();

            this.CopyNonIdentifiers(item, result);

            return result;
        }

        /// <summary>
        /// [Tested]
        /// </summary> 
        public abstract void CopyNonIdentifiers(TPoco source, TPoco destination);

        /// <summary>
        /// [Tested]
        /// </summary> 
        public void CopyNonIdentifiers(List<TPoco> source, List<TPoco> destination)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (ReferenceEquals(source, destination)) throw new Exception($"{nameof(source)} and {nameof(destination)} are the same list.");
            if (source.Count != destination.Count) throw new Exception($"{nameof(source)} and {nameof(destination)} are not the same lenght.");

            for (var i=0; i<source.Count(); i++)
            {
                this.CopyNonIdentifiers(source[i], destination[i]);
            }
        }

        #endregion

        #region item non-identifier manipulation

        /// <summary>
        ///     item must be changed so that its non-identifers fields do not match any of the same fields on otherItems
        ///     [Tested]
        /// </summary>
        /// <param name="item"></param>
        /// <param name="otherItems"></param>
        public abstract void ChangeNonIdentifiers(TPoco item, IEnumerable<TPoco> otherItems);

        /// <summary>
        /// [Tested]
        /// </summary>
        public void ChangeNonIdentifiers(IEnumerable<TPoco> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            foreach (var item in items)
            {
                var otherItems = items.Where(i => !ReferenceEquals(i, item));

                ChangeNonIdentifiers(item, otherItems);
            }
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public void ChangeNonIdentifiers(TPoco item, TPoco otherItem)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (otherItem == null) throw new ArgumentNullException(nameof(otherItem));
            if (ReferenceEquals(item, otherItem)) throw new Exception($"{nameof(item)} is the same reference as {nameof(otherItem)}");

            this.ChangeNonIdentifiers(item, new List<TPoco> { otherItem });
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public void ChangeNonIdentifiers(IEnumerable<TPoco> items, IEnumerable<TPoco> otherItems)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (otherItems == null) throw new ArgumentNullException(nameof(otherItems));

            if (items.Any(i => otherItems.Any(oi => ReferenceEquals(i, oi))))
            {
                throw new Exception($"No element in {nameof(items)} can reference anything in {nameof(otherItems)}");
            }

            foreach (var item in items)
            {
                this.ChangeNonIdentifiers(item, otherItems);
            }
        }         

        #endregion

        #region identity manipulation

        /// <summary>
        /// [Tested]
        /// </summary>
        public abstract IEnumerable<IIdentity> MakeAllowedIdentities();

        /// <summary>
        /// [Tested]
        /// </summary>
        public abstract IEnumerable<IIdentity> MakeDeniedIdentities();

        /// <summary>
        /// [Tested]
        /// </summary>
        public virtual IEnumerable<IIdentity> MakeInvalidIdentities()
        {
            return new List<Identity> { null, new Identity() };
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public abstract IIdentity GetAdminIdentity();

        #endregion

        #region crud operations

        /// <summary>
        /// [Tested]
        /// </summary>
        public IList<TPoco> AddItems(List<TPoco> validItems)
        {
            if (validItems == null) throw new ArgumentNullException(nameof(validItems));

            var admin = this.GetAdminIdentity();
            var before = this.GetAllItems().Count();
            var response = boCreater.AddSome(validItems, admin);

            if (!this.ResponseIsOk(response)) throw new Exception($"{nameof(AddItems)} failed to add items.");
            if (response.Items == null) throw new Exception($"{nameof(AddItems)} failed to return items.");

            var after = this.GetAllItems().Count();

            if (after != before + validItems.Count()) throw new Exception($"{nameof(AddItems)} failed to all the provided items.");

            return response.Items.ToList();
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public IList<TPoco> AddValidItems(IEnumerable<TPoco> otherItems)
        {
            var validItems = this.MakeValidItems(Keys.GenIfMnl, otherItems);

            return this.AddItems(validItems.ToList());
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public IList<TPoco> AddValidItems(TPoco otherItem)
        {
            var validItems = this.MakeValidItems(Keys.GenIfMnl, new List<TPoco> { otherItem });

            return this.AddItems(validItems.ToList());
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public IList<TPoco> AddValidItems()
        {
            return this.AddValidItems(this.DefaultRequiredValidItems);
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public IList<TPoco> AddValidItems(int required)
        {
            var validItems = this.MakeValidItems(required, Keys.GenIfMnl, new List<TPoco>());

            return this.AddItems(validItems.ToList());
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public TPoco AddItem(TPoco validItem)
        {
            if (validItem == null) throw new ArgumentNullException(nameof(validItem));

            var admin = this.GetAdminIdentity();
            var response = boCreater.Add(validItem, admin);

            if (!this.ResponseIsOk(response)) throw new Exception($"{nameof(AddItem)} could not add the item.");
            if (response.Item == null) throw new Exception($"{nameof(AddItem)} was not returned an item.");
            if (!this.ItemExists(response.Item)) throw new Exception($"{nameof(AddItem)} the added item could not be found.");

            return response.Item;
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public TPoco GetItem(TPoco exitingItem)
        {
            if (exitingItem == null) throw new ArgumentNullException(nameof(exitingItem));

            var admin = this.GetAdminIdentity();
            var response = boReader.GetByKey(exitingItem, admin);

            if (!this.ResponseIsOk(response)) throw new Exception($"{nameof(GetItem)} could not look for the item.");
            if (response.Item == null) throw new Exception($"{nameof(GetItem)} was not returned an item.");
            if (!this.IdentifiersAreEqual(exitingItem, response.Item)) throw new Exception($"{nameof(GetItem)} was returned an item whos identifiers do not match the request.");

            return response.Item;
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public bool ItemExists(TPoco validItem)
        {
            if (validItem == null) throw new ArgumentNullException(nameof(validItem));

            var admin = this.GetAdminIdentity();
            var response = boReader.ExistsByKey(validItem, admin);
            
            if (!this.ResponseIsOk(response)) throw new Exception($"{nameof(ItemExists)} could not look for the item.");

            return response.Item;
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public void RemoveAllItems()
        {
            var admin = this.GetAdminIdentity();
            var allResponse = boReader.GetAll(admin);

            if (!this.ResponseIsOk(allResponse)) throw new Exception($"{nameof(RemoveAllItems)} could not look for all items to remove.");
            if (allResponse.Items == null) throw new Exception($"{nameof(RemoveAllItems)} was not returned any items.");

            var deleteResponse = boDeleter.DeleteSomeByKey(allResponse.Items, admin);

            if (!this.ResponseIsOk(deleteResponse)) throw new Exception($"{nameof(RemoveAllItems)} could not delete all items.");
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public IList<TPoco> GetAllItems()
        {
            var admin = this.GetAdminIdentity();
            var allResponse = boReader.GetAll(admin);

            if (!this.ResponseIsOk(allResponse)) throw new Exception($"{nameof(GetAllItems)} could not look for all items.");
            if (allResponse.Items == null) throw new Exception($"{nameof(GetAllItems)} was not returned any items.");
            
            return allResponse.Items;
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public bool ItemCountIs(int count)
        {
            return this.GetAllItems().Count() == count;
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public bool HasNoItems()
        {
            return this.ItemCountIs(0);
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public bool StoredItemsAreEqualTo(IList<TPoco> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            var storedItems = this.GetAllItems();
            
            if (items.Count != storedItems.Count)
            {
                return false;
            }

            return items.All(i => storedItems.Any(si => this.AreEqual(i, si)));
        }

        #endregion

        #region response investigatoin

        /// <summary>
        /// [Tested]
        /// </summary>
        public bool ResponseIsOk(IResponse response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            if (response == null)
            {
                return false;
            }

            return response.Ok;            
        }

        /// <summary>
        /// [Tested]
        /// </summary>
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
            this.Test_DefaultRequiredValidItems();
            this.Test_DefaultRequiredInvalidItems();
            this.Test_DefaultRequiredItems();
            this.Test_MakeValidItems();
            this.Test_MakeInvalidItems();
            this.Test_MakeInvalidItem();
            this.Test_AreEqual();
            this.Test_Contains();
            this.Test_ContainsIdentifiers();
            this.Test_ContainsNonIdentifiers();
            this.Test_IdentifiersAreEqual();
            this.Test_NonIdentifiersAreEqual();
            this.Test_Copy();
            this.Test_CopyIdentifiers();
            this.Test_CopyNonIdentifiers();
            this.Test_ChangeNonIdentifiers();
            this.Test_ChangeProperty();
            this.Test_MakeKeyValue();
            this.Test_MakeAllowedIdentities();
            this.Test_MakeDeniedIdentities();
            this.Test_MakeInvalidIdentities();
            this.Test_GetAdminIdentity();
            this.Test_AddItems();
            this.Test_AddValidItems();
            this.Test_AddItem();
            this.Test_GetItem();
            this.Test_ItemExists();
            this.Test_RemoveAllItems();
            this.Test_GetAllItems();
            this.Test_ItemCountIs();
            this.Test_HasNoItems();
            this.Test_StoredItemsAreEqualTo();
            this.Test_ResponseIsOk();
            this.Test_ResponseFailsWithError();
        }

        public void Test_DefaultRequiredValidItems()
        {
            Assert.That(this.DefaultRequiredValidItems, Is.GreaterThan(1));
        }

        public void Test_DefaultRequiredInvalidItems()
        {
            Assert.That(this.DefaultRequiredInvalidItems, Is.GreaterThan(1));
        }
        
        public void Test_DefaultRequiredItems()
        {
            var a = this.DefaultRequiredValidItems;
            var b = this.DefaultRequiredInvalidItems;
            var greater = a > b ? a : b;

            Assert.That(this.DefaultRequiredItems, Is.EqualTo(greater));
        }

        public void Test_MakeValidItems()
        {
            var defaultItem = new TPoco();

            var result = MakeValidItems(Keys.Gen);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(this.DefaultRequiredValidItems));
            Assert.That(result.Any(i => i == null), Is.False);
            Assert.That(HasDuplicateReference(result), Is.False);
            Assert.That(result.All(i => !this.IdentifiersAreEqual(defaultItem, i)), Is.True);
            var result1 = result.Select((v, i) => new { v, i });
            var result2 = result.Select((v, i) => new { v, i });
            Assert.That(result1.All(r1 => result2.Where(r2 => r1.i != r2.i).All(r2 => !this.IdentifiersAreEqual(r1.v, r2.v))), Is.True);

            result = MakeValidItems(Keys.Default);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(this.DefaultRequiredValidItems));
            Assert.That(result.Any(i => i == null), Is.False);
            Assert.That(HasDuplicateReference(result), Is.False);
            Assert.That(result.All(i => this.IdentifiersAreEqual(defaultItem, i)), Is.True);

            var twice = this.DefaultRequiredValidItems * 2;

            result = MakeValidItems(twice, Keys.Gen);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(twice));
            Assert.That(result.Any(i => i == null), Is.False);
            Assert.That(HasDuplicateReference(result), Is.False);
            Assert.That(result.All(i => !this.IdentifiersAreEqual(defaultItem, i)), Is.True);
            result1 = result.Select((v, i) => new { v, i });
            result2 = result.Select((v, i) => new { v, i });
            Assert.That(result1.All(r1 => result2.Where(r2 => r1.i != r2.i).All(r2 => !this.IdentifiersAreEqual(r1.v, r2.v))), Is.True);

            result = MakeValidItems(twice, Keys.GenIfMnl);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(twice));
            Assert.That(result.Any(i => i == null), Is.False);
            Assert.That(HasDuplicateReference(result), Is.False);
            Assert.That(result.All(i => this.IdentifiersAreEqual(defaultItem, i)), Is.True);
            //Assert.That(result.All(r1 => result.Where(r2 => !ReferenceEquals(r1, r2)).All(r2 => !this.IdentifiersAreEqual(r1, r2))), Is.True);

            result = MakeValidItems(twice, Keys.Default);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(twice));
            Assert.That(result.Any(i => i == null), Is.False);
            Assert.That(HasDuplicateReference(result), Is.False);
            Assert.That(result.All(i => this.IdentifiersAreEqual(defaultItem, i)), Is.True);

            var otherItems = MakeValidItems(Keys.Gen);

            result = MakeValidItems(Keys.Gen, otherItems);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(this.DefaultRequiredValidItems));
            Assert.That(result.Any(i => i == null), Is.False);
            Assert.That(HasDuplicateReference(result), Is.False);
            Assert.That(result.All(i => !this.IdentifiersAreEqual(defaultItem, i)), Is.True);
            Assert.That(result.All(i => !otherItems.Any(oi => this.IdentifiersAreEqual(i, oi))), Is.True);
            result1 = result.Select((v, i) => new { v, i });
            result2 = result.Select((v, i) => new { v, i });
            Assert.That(result1.All(r1 => result2.Where(r2 => r1.i != r2.i).All(r2 => !this.IdentifiersAreEqual(r1.v, r2.v))), Is.True);

            result = MakeValidItems(Keys.Default, otherItems);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(this.DefaultRequiredValidItems));
            Assert.That(result.Any(i => i == null), Is.False);
            Assert.That(HasDuplicateReference(result), Is.False);
            Assert.That(result.All(i => this.IdentifiersAreEqual(defaultItem, i)), Is.True);
        }

        public void Test_MakeInvalidItems()
        {
            var items = MakeValidItems(Keys.Gen).ToList();
            var original = this.Copy(items);
            MakeInvalidItems(items);
            Assert.That(items.Count(), Is.EqualTo(original.Count()));
            Assert.That(items.Count(i => i == null), Is.EqualTo(0));
            Assert.That(HasDuplicateReference(items), Is.False);
            for (var i = 0; i < original.Count(); i++)
            {
                Assert.That(this.IdentifiersAreEqual(original.ElementAt(i), items[i]), Is.True);
            }
            Assert.That(items.All(i => original.Any(o => !this.NonIdentifiersAreEqual(i, o))), Is.True);

            items = MakeValidItems(Keys.Gen).ToList();
            var otherItems = MakeValidItems(Keys.Gen, items);            
            original = this.Copy(items);
            MakeInvalidItems(items, otherItems);
            Assert.That(items.Count(), Is.EqualTo(original.Count()));
            Assert.That(items.Count(i => i == null), Is.EqualTo(0));
            Assert.That(HasDuplicateReference(items), Is.False);
            for (var i = 0; i < original.Count(); i++)
            {
                Assert.That(this.IdentifiersAreEqual(original.ElementAt(i), items[i]), Is.True);
            }
            Assert.That(items.All(i => otherItems.Any(o => !this.NonIdentifiersAreEqual(i, o))), Is.True);

            items = MakeValidItems(Keys.Gen).ToList();
            otherItems = MakeValidItems(Keys.Gen, items);
            original = this.Copy(items);
            MakeInvalidItems(true, items, otherItems);
            Assert.That(items.Count(), Is.EqualTo(original.Count() + 1));
            Assert.That(items.Count(i => i == null), Is.EqualTo(1));
            Assert.That(HasDuplicateReference(items), Is.False);
            for (var i = 0; i < original.Count(); i++)
            {
                if (items[i] == null) continue;

                Assert.That(this.IdentifiersAreEqual(original.ElementAt(i), items[i]), Is.True);
            }
            Assert.That(items.All(i => i == null || otherItems.Any(o => !this.NonIdentifiersAreEqual(i, o))), Is.True);
            
            var result = this.MakeInvalidItems(false, Keys.Gen);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(this.DefaultRequiredInvalidItems));
            Assert.That(HasDuplicateReference(result), Is.False);

            result = this.MakeInvalidItems(Keys.Gen);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(this.DefaultRequiredInvalidItems));
            Assert.That(HasDuplicateReference(result), Is.False);

            result = this.MakeInvalidItems(false, this.DefaultRequiredInvalidItems * 2, Keys.Gen);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(this.DefaultRequiredInvalidItems * 2));
            Assert.That(HasDuplicateReference(result), Is.False);

            result = this.MakeInvalidItems(true, Keys.Gen);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(this.DefaultRequiredInvalidItems + 1));
            Assert.That(HasDuplicateReference(result), Is.False);

            result = this.MakeInvalidItems(Keys.Gen, otherItems);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(this.DefaultRequiredInvalidItems));
            Assert.That(HasDuplicateReference(result), Is.False);
            Assert.That(result.All(i => otherItems.Any(o => !this.NonIdentifiersAreEqual(i, o))), Is.True);
        }

        public void Test_MakeInvalidItem()
        {
            foreach (var validItem in this.MakeValidItems(Keys.Gen))
            {
                var item = this.Copy(validItem);
                MakeInvalidItem(item);

                Assert.That(validItem, Is.Not.Null);
                Assert.That(this.NonIdentifiersAreEqual(validItem, item), Is.False);
            }
        }

        public void Test_AreEqual()
        {
            var itemsA = this.MakeValidItems(Keys.Gen);
            var itemsB = this.Copy(itemsA);

            for (var i = 0; i<itemsA.Count(); i++)
            {
                var itemA = itemsA.ElementAt(i);
                var copyA = this.Copy(itemA);
                var itemB = itemsA.ElementAt(i);
                var copyB = this.Copy(itemB);

                Assert.That(this.AreEqual(itemA, copyA), Is.True);
                Assert.That(this.AreEqual(itemB, copyB), Is.True);
                Assert.That(this.AreEqual(itemA, itemB), Is.True);
                Assert.That(this.IdentifiersAreEqual(itemA, itemB), Is.True);
                Assert.That(this.NonIdentifiersAreEqual(itemA, itemB), Is.True);
            }

            Assert.That(this.AreEqual(itemsA, itemsB), Is.True);
        }

        public void Test_Contains()
        {
            var items1 = this.MakeValidItems(Keys.Gen);
            var items2 = this.Copy(items1);
            var empty = new List<TPoco>();
            var partial = new List<TPoco> { items1.First() };
            var different = this.MakeValidItems(Keys.Gen, items1);

            Assert.That(this.Contains(items1, items2), Is.True);
            Assert.That(this.Contains(items2, items1), Is.True);
            Assert.That(this.Contains(items1, partial), Is.True);
            Assert.That(this.Contains(partial, items1), Is.False);
            Assert.That(this.Contains(items1, empty), Is.True);
            Assert.That(this.Contains(empty, items1), Is.False);
            Assert.That(this.Contains(empty, empty), Is.True);
            Assert.That(this.Contains(different, items1), Is.False);
        }

        public void Test_ContainsIdentifiers()
        {
            var items1 = this.MakeValidItems(Keys.Gen);
            var items2 = this.Copy(items1);
            this.ChangeNonIdentifiers(items1);
            var empty = new List<TPoco>();
            var partial = new List<TPoco> { items1.First() };
            var different = this.MakeValidItems(Keys.Gen, items1);

            Assert.That(this.ContainsIdentifiers(items1, items2), Is.True);
            Assert.That(this.ContainsIdentifiers(items2, items1), Is.True);
            Assert.That(this.ContainsIdentifiers(items1, partial), Is.True);
            Assert.That(this.ContainsIdentifiers(partial, items1), Is.False);
            Assert.That(this.ContainsIdentifiers(items1, empty), Is.True);
            Assert.That(this.ContainsIdentifiers(empty, items1), Is.False);
            Assert.That(this.ContainsIdentifiers(empty, empty), Is.True);
            Assert.That(this.ContainsIdentifiers(different, items1), Is.False);
        }

        public void Test_ContainsNonIdentifiers()
        {
            var items1 = this.MakeValidItems(Keys.Gen).ToList();
            var items2 = this.MakeValidItems(Keys.Gen, items1).ToList();
            this.CopyNonIdentifiers(items1, items2);
            var empty = new List<TPoco>();
            var partial = new List<TPoco> { items1.First() };
            var different = this.MakeValidItems(Keys.Gen, items1);

            Assert.That(this.ContainsNonIdentifiers(items1, items2), Is.True);
            Assert.That(this.ContainsNonIdentifiers(items2, items1), Is.True);
            Assert.That(this.ContainsNonIdentifiers(items1, partial), Is.True);
            Assert.That(this.ContainsNonIdentifiers(partial, items1), Is.False);
            Assert.That(this.ContainsNonIdentifiers(items1, empty), Is.True);
            Assert.That(this.ContainsNonIdentifiers(empty, items1), Is.False);
            Assert.That(this.ContainsNonIdentifiers(empty, empty), Is.True);
            Assert.That(this.ContainsNonIdentifiers(different, items1), Is.False);
        }

        public void Test_IdentifiersAreEqual()
        {
            foreach (var itemA in this.MakeValidItems(Keys.Gen))
            {
                var originalItemA = this.Copy(itemA);
                var itemB = this.Copy(itemA);
                var originalItemB = this.Copy(itemB);

                Assert.That(this.IdentifiersAreEqual(itemA, itemB), Is.True);
                Assert.That(this.AreEqual(itemA, originalItemA), Is.True);
                Assert.That(this.AreEqual(itemB, originalItemB), Is.True);

                var itemsC = this.MakeValidItems(Keys.Gen).ToList();
                var originalItemsC = this.Copy(itemsC);
                var itemsD = this.Copy(itemsC);
                var originalItemsD = this.Copy(itemsD);

                Assert.That(this.IdentifiersAreEqual(itemsC, itemsD), Is.True);
                Assert.That(this.AreEqual(itemsC, originalItemsC), Is.True);
                Assert.That(this.AreEqual(itemsD, originalItemsD), Is.True);
            }
        }

        public void Test_NonIdentifiersAreEqual()
        {
            foreach (var itemA in this.MakeValidItems(Keys.Gen))
            {
                var originalItemA = this.Copy(itemA);
                var itemB = this.Copy(itemA);
                var originalItemB = this.Copy(itemB);

                Assert.That(this.NonIdentifiersAreEqual(itemA, itemB), Is.True);
                Assert.That(this.AreEqual(itemA, originalItemA), Is.True);
                Assert.That(this.AreEqual(itemB, originalItemB), Is.True);

                var itemsC = this.MakeValidItems(Keys.Gen).ToList();
                var originalItemsC = this.Copy(itemsC);
                var itemsD = this.Copy(itemsC);
                var originalItemsD = this.Copy(itemsD);

                Assert.That(this.NonIdentifiersAreEqual(itemsC, itemsD), Is.True);
                Assert.That(this.AreEqual(itemsC, originalItemsC), Is.True);
                Assert.That(this.AreEqual(itemsD, originalItemsD), Is.True);
            }
        }

        public void Test_Copy()
        {
            foreach (var validItem in this.MakeValidItems(Keys.Gen))
            {
                var copiedItem = this.Copy(validItem);

                Assert.That(ReferenceEquals(copiedItem, validItem), Is.False);
                Assert.That(this.AreEqual(validItem, copiedItem));
            }

            var validItems = this.MakeValidItems(Keys.Gen);
            var orginalCount = validItems.Count();
            var copiedItems = this.Copy(validItems);

            Assert.That(copiedItems, Is.Not.Null);
            Assert.That(copiedItems.Count, Is.EqualTo(orginalCount));
            Assert.That(this.AreEqual(validItems, copiedItems), Is.True);
            Assert.That(HasDuplicateReference(validItems, copiedItems), Is.False);

            foreach (var validItem in this.MakeValidItems(Keys.Gen))
            {
                var copiedItem = new TPoco();
                this.Copy(validItem, copiedItem);

                Assert.That(this.AreEqual(validItem, copiedItem), Is.True);
            }
        }

        public void Test_CopyIdentifiers()
        {
            foreach (var validItem in this.MakeValidItems(Keys.Gen))
            {
                var defaultItem = new TPoco();                
                var copiedItem = this.CopyIdentifiers(validItem);

                Assert.That(copiedItem, Is.Not.Null);
                Assert.That(ReferenceEquals(copiedItem, validItem), Is.False);
                Assert.That(this.IdentifiersAreEqual(copiedItem, validItem), Is.True);
                Assert.That(this.NonIdentifiersAreEqual(copiedItem, defaultItem), Is.True);
            }

            foreach (var validItem in this.MakeValidItems(Keys.Gen))
            {
                var defaultItem = new TPoco();
                this.ChangeNonIdentifiers(defaultItem, validItem);
                this.CopyIdentifiers(validItem, defaultItem);

                Assert.That(this.IdentifiersAreEqual(validItem, defaultItem), Is.True);
                Assert.That(this.NonIdentifiersAreEqual(validItem, defaultItem), Is.False);
            }
        }

        public void Test_CopyNonIdentifiers()
        {
            var defaultItem = new TPoco();

            foreach (var validItem in this.MakeValidItems(Keys.Gen))
            {
                var copiedItem = this.CopyNonIdentifiers(validItem);

                Assert.That(copiedItem, Is.Not.Null);
                Assert.That(ReferenceEquals(copiedItem, validItem), Is.False);
                Assert.That(this.IdentifiersAreEqual(copiedItem, defaultItem), Is.True);
                Assert.That(this.NonIdentifiersAreEqual(copiedItem, validItem), Is.True);
            }

            foreach (var validItem in this.MakeValidItems(Keys.Gen))
            {
                this.ChangeNonIdentifiers(defaultItem, validItem);
                this.CopyNonIdentifiers(validItem, defaultItem);

                Assert.That(this.IdentifiersAreEqual(validItem, defaultItem), Is.False);
                Assert.That(this.NonIdentifiersAreEqual(validItem, defaultItem), Is.True);
            }

            var validItems = this.MakeValidItems(Keys.Gen).ToList();
            var destination = new List<TPoco>();

            for (var i=0; i<validItems.Count(); i++)
            {
                destination.Add(new TPoco());
            }

            this.CopyNonIdentifiers(validItems, destination);

            Assert.That(HasDuplicateReference(validItems, destination), Is.False);

            for (var i = 0; i < validItems.Count(); i++)
            {
                Assert.That(this.IdentifiersAreEqual(validItems[i], destination[i]), Is.False);
                Assert.That(this.NonIdentifiersAreEqual(validItems[i], destination[i]), Is.True);
            }
        }

        public void Test_ChangeNonIdentifiers()
        {
            var validItems = this.MakeValidItems(Keys.Gen).ToList();

            for (var t = 0; t < validItems.Count(); t++)
            {
                var others = new List<TPoco>();
                var target = validItems[t];
                var original = this.Copy(validItems[t]);

                for (var i = 0; i < validItems.Count(); i++)
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

            var list1 = this.MakeValidItems(Keys.Gen).ToList();
            var list2 = this.Copy(list1).ToList();

            for (var i = 0; i<list1.Count(); i++)
            {
                var i1 = list1[i];
                var i2 = list2[i];
                this.ChangeNonIdentifiers(i1, i2);

                Assert.That(this.IdentifiersAreEqual(i1, i2), Is.True);
                Assert.That(this.NonIdentifiersAreEqual(i1, i2), Is.False);
            }

            list1 = this.MakeValidItems(Keys.Gen).ToList();
            list2 = this.Copy(list1).ToList();
            this.ChangeNonIdentifiers(list1);            

            for (var i = 0; i < list1.Count(); i++)
            {
                var i1 = list1[i];
                var i2 = list2[i];

                Assert.That(this.IdentifiersAreEqual(i1, i2), Is.True);
                Assert.That(this.NonIdentifiersAreEqual(i1, i2), Is.False);
            }

            list1 = this.MakeValidItems(Keys.Gen).ToList();
            list2 = this.Copy(list1).ToList();
            this.ChangeNonIdentifiers(list1, list2);
            Assert.That(list1.All(i1 => !list2.Any(i2 => this.NonIdentifiersAreEqual(i1, i2))), Is.True);

            for (var i = 0; i < list1.Count(); i++)
            {
                var i1 = list1[i];
                var i2 = list2[i];

                Assert.That(this.IdentifiersAreEqual(i1, i2), Is.True);
            }
        }        

        public void Test_ChangeProperty()
        {
            var pattern = "thing {0} x";
            var current = "thing 1 x";
            var otherValues = new List<string> { "thing 2 x", "thing 3 x" };
            var result = ChangeProperty(current, otherValues, pattern);
            Assert.That(result, Is.EqualTo("thing 4 x"));

            otherValues = new List<string> { "thing 3 x", "thing 4 x" };
            result = ChangeProperty(current, otherValues, pattern);
            Assert.That(result, Is.EqualTo("thing 2 x"));

            result = ChangeProperty(otherValues, pattern);
            Assert.That(result, Is.EqualTo("thing 1 x"));

            otherValues = new List<string> { "thing 1 x", "thing 2 x" };
            result = ChangeProperty(otherValues, pattern);
            Assert.That(result, Is.EqualTo("thing 3 x"));

            var current2 = 5;
            var otherValues2 = new List<int> { 4, 7, 3 };
            var result2 = this.ChangeProperty(current2, otherValues2);
            Assert.That(result2, Is.EqualTo(6));

            current2 = 7;
            result2 = this.ChangeProperty(current2, otherValues2);
            Assert.That(result2, Is.EqualTo(8));

            result2 = this.ChangeProperty(otherValues2);
            Assert.That(result2, Is.EqualTo(1));
        }

        public void Test_MakeKeyValue()
        {
            var others = new List<int?> { 1, 4, 7, 3 };

            var result = this.MakeKeyValue(Keys.Default, false, others);
            Assert.That(result, Is.EqualTo(0));

            result = this.MakeKeyValue(Keys.Default, true, others);
            Assert.That(result, Is.EqualTo(0));

            result = this.MakeKeyValue(Keys.Gen, false, others);
            Assert.That(result, Is.EqualTo(2));

            result = this.MakeKeyValue(Keys.Gen, true, others);
            Assert.That(result, Is.EqualTo(2));

            result = this.MakeKeyValue(Keys.GenIfMnl, false, others);
            Assert.That(result, Is.EqualTo(2));

            result = this.MakeKeyValue(Keys.GenIfMnl, true, others);
            Assert.That(result, Is.EqualTo(0));
        }

        public void Test_MakeAllowedIdentities()
        {
            var allowedIdentities = this.MakeAllowedIdentities().ToList();

            Assert.That(allowedIdentities, Is.Not.Null);
            Assert.That(allowedIdentities.Count(), Is.GreaterThan(0));
            Assert.That(allowedIdentities.Count(i => i == null), Is.EqualTo(0));
            Assert.That(HasDuplicateReference(allowedIdentities), Is.False);

            foreach (var identity in allowedIdentities)
            {
                this.RemoveAllItems();
                var validItem = this.MakeValidItems(Keys.GenIfMnl).First();
                var result = this.boCreater.Add(validItem, identity);

                Assert.That(this.ResponseIsOk(result), Is.True);
            }
        }

        public void Test_MakeDeniedIdentities()
        {
            var deniedIdentities = this.MakeDeniedIdentities().ToList();

            Assert.That(deniedIdentities, Is.Not.Null);
            Assert.That(deniedIdentities.Count(), Is.GreaterThan(0));
            Assert.That(deniedIdentities.Count(i => i == null), Is.EqualTo(0));
            Assert.That(HasDuplicateReference(deniedIdentities), Is.False);

            foreach (var identity in deniedIdentities)
            {
                this.RemoveAllItems();
                var validItem = this.MakeValidItems(Keys.GenIfMnl).First();
                var result = this.boCreater.Add(validItem, identity);

                Assert.That(this.ResponseFailsWithError(result, BusinessCore.UserDeniedCode));
            }
        }

        public void Test_MakeInvalidIdentities()
        {
            var invalidIdentities = this.MakeInvalidIdentities().ToList();

            Assert.That(invalidIdentities, Is.Not.Null);
            Assert.That(invalidIdentities.Count(), Is.GreaterThan(1));
            Assert.That(invalidIdentities.Count(i => i == null), Is.EqualTo(1));
            Assert.That(HasDuplicateReference(invalidIdentities), Is.False);

            foreach (var identity in invalidIdentities)
            {
                this.RemoveAllItems();
                var validItem = this.MakeValidItems(Keys.GenIfMnl).First();
                var result = this.boCreater.Add(validItem, identity);

                Assert.That(this.ResponseFailsWithError(result, BusinessCore.InvalidIdentityCode));
            }
        }

        public void Test_GetAdminIdentity()
        {
            var identity = this.GetAdminIdentity();

            Assert.That(identity, Is.Not.Null);

            this.RemoveAllItems();
            var validItem = this.MakeValidItems(Keys.GenIfMnl).First();
            var response = this.boCreater.Add(validItem, identity) as IResponse;
            Assert.That(this.ResponseIsOk(response), Is.True);

            this.RemoveAllItems();
            var validItems = this.MakeValidItems(Keys.GenIfMnl);
            response = this.boCreater.AddSome(validItems, identity);
            Assert.That(this.ResponseIsOk(response), Is.True);

            this.RemoveAllItems();
            validItems = this.AddValidItems();
            response = this.boDeleter.DeleteByKey(validItems.First(), identity);
            Assert.That(this.ResponseIsOk(response), Is.True);

            this.RemoveAllItems();
            validItems = this.AddValidItems();
            response = this.boDeleter.DeleteSomeByKey(validItems, identity);
            Assert.That(this.ResponseIsOk(response), Is.True);

            this.RemoveAllItems();
            validItems = this.AddValidItems();

            response = this.boReader.ExistsByKey(validItems.First(), identity);
            Assert.That(this.ResponseIsOk(response), Is.True);
            
            response = this.boReader.GetByKey(validItems.First(), identity);
            Assert.That(this.ResponseIsOk(response), Is.True);
            
            response = this.boReader.GetByKey(validItems.First(), identity);
            Assert.That(this.ResponseIsOk(response), Is.True);

            //Todo: test that search by works for admin
        }

        public void Test_AddItems()
        {
            this.RemoveAllItems();

            var validItems = this.MakeValidItems(Keys.GenIfMnl).ToList();

            var items = this.AddItems(validItems);

            Assert.That(validItems.Count(), Is.EqualTo(items.Count()));
            Assert.That(validItems.Count(), Is.EqualTo(this.DefaultRequiredValidItems));
        }

        public void Test_AddValidItems()
        {
            this.RemoveAllItems();

            {
                var items = this.AddValidItems();

                Assert.That(items, Is.Not.Null);
                Assert.That(items.Count(), Is.GreaterThan(1));
                Assert.That(items.Count(i => i == null), Is.EqualTo(0));
                Assert.That(items.Count(), Is.EqualTo(this.DefaultRequiredValidItems));
            }

            {
                var required = this.DefaultRequiredValidItems * 2;
                var items = this.AddValidItems(required);

                Assert.That(items, Is.Not.Null);
                Assert.That(items.Count(), Is.EqualTo(required));
                Assert.That(items.Count(i => i == null), Is.EqualTo(0));
            }

            foreach (var item in this.MakeValidItems(Keys.GenIfMnl))
            {
                this.RemoveAllItems();

                var items = this.AddValidItems(item);

                Assert.That(items, Is.Not.Null);
                Assert.That(items.Count(), Is.GreaterThan(1));
                Assert.That(items.Count(i => i == null), Is.EqualTo(0));
                Assert.That(items.Count(), Is.EqualTo(this.DefaultRequiredValidItems));
                Assert.That(items.All(i => !this.AreEqual(i, item)), Is.True);
            }

            {
                this.RemoveAllItems();

                var otherItems = this.MakeValidItems(Keys.GenIfMnl);
                var items = this.AddValidItems(otherItems);

                Assert.That(items, Is.Not.Null);
                Assert.That(items.Count(), Is.GreaterThan(1));
                Assert.That(items.Count(i => i == null), Is.EqualTo(0));
                Assert.That(items.Count(), Is.EqualTo(this.DefaultRequiredValidItems));
                Assert.That(items.All(i => otherItems.All(o => !this.AreEqual(i, o))), Is.True);

            }
        }

        public void Test_AddItem()
        {
            this.RemoveAllItems();

            var validItem = this.MakeValidItems(Keys.GenIfMnl).First();

            var addedItem = this.AddItem(validItem);

            Assert.That(addedItem, Is.Not.Null);
        }

        public void Test_GetItem()
        {
            this.RemoveAllItems();
            
            var addedItems = this.AddValidItems();

            var first = this.GetItem(addedItems.First());

            Assert.That(this.AreEqual(addedItems.First(), first), Is.True);
        }

        public void Test_ItemExists()
        {
            this.RemoveAllItems();

            var addedItems = this.AddValidItems();

            var first = this.ItemExists(addedItems.First());

            Assert.That(first, Is.True);
        }

        public void Test_RemoveAllItems()
        {
            this.RemoveAllItems();

            var allItems = this.GetAllItems();

            Assert.That(allItems.Count(), Is.EqualTo(0));

            this.AddValidItems();

            allItems = this.GetAllItems();

            Assert.That(allItems.Count(), Is.GreaterThan(0));

            this.RemoveAllItems();

            allItems = this.GetAllItems();

            Assert.That(allItems.Count(), Is.EqualTo(0));
        }

        public void Test_GetAllItems()
        {
            this.RemoveAllItems();

            var addedItems = this.AddValidItems();

            var allItems = this.GetAllItems();

            Assert.That(allItems.Count(), Is.EqualTo(addedItems.Count()));
            Assert.That(HasDuplicateReference(allItems), Is.False);
        }

        public void Test_ItemCountIs()
        {
            this.RemoveAllItems();

            this.AddValidItems();

            var count = this.GetAllItems().Count();

            Assert.That(this.ItemCountIs(count), Is.True);
            Assert.That(this.ItemCountIs(count + 1), Is.False);
        }

        public void Test_HasNoItems()
        {
            this.RemoveAllItems();

            Assert.That(this.HasNoItems(), Is.True);

            this.AddValidItems();

            Assert.That(this.HasNoItems(), Is.False);
        }

        public void Test_StoredItemsAreEqualTo()
        {
            this.RemoveAllItems();

            var items1 = this.AddValidItems();
            var items2 = this.Copy(items1).ToList();
            var items3= this.Copy(items1).ToList();

            Assert.That(this.StoredItemsAreEqualTo(items2), Is.True);
            Assert.That(this.StoredItemsAreEqualTo(new List<TPoco>()), Is.False);

            items2.RemoveAt(items2.Count() - 1);

            Assert.That(this.StoredItemsAreEqualTo(items2), Is.False);

            this.ChangeNonIdentifiers(items3);

            Assert.That(this.StoredItemsAreEqualTo(items3), Is.False);
        }        

        public void Test_ResponseIsOk()
        {
            var response1 = new Response();

            MessageUtility.Errors.Add("A message", "A code", response1);

            Assert.That(this.ResponseIsOk(response1), Is.False);

            var response2 = new Response();

            Assert.That(this.ResponseIsOk(response2), Is.True);
        }

        public void Test_ResponseFailsWithError()
        {
            var response1 = new Response();

            MessageUtility.Errors.Add("A message", "A code", response1);

            Assert.That(this.ResponseFailsWithError(response1, "A code"), Is.True);
            Assert.That(this.ResponseFailsWithError(response1, "Another code"), Is.False);

            var response2 = new Response();

            Assert.That(this.ResponseFailsWithError(response2, "A code"), Is.False);
        }

        public static bool HasDuplicateReference(IEnumerable<object> list)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));            

            for (var a = 0; a < list.Count(); a++)
            {
                for (var b = 0; b < list.Count(); b++)
                {
                    if (a == b) continue;

                    if (ReferenceEquals(list.ElementAt(a), list.ElementAt(b)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool HasDuplicateReference(IEnumerable<TPoco> list1, IEnumerable<TPoco> list2)
        {
            if (list1.Any(i1 => list2.Any(i2 => ReferenceEquals(i1, i2))))
            {
                return true;
            }

            if (list2.Any(i2 => list1.Any(i1 => ReferenceEquals(i1, i2))))
            {
                return true;
            }

            return false;
        }        

        #endregion
    }

    public class TestSuitHelper
    {
        public enum Keys
        {
            /// <summary>
            /// Generate new key(s) for the new Poco regardless of it auto generation nature.
            /// </summary>
            Gen,

            /// <summary>
            /// Generate new key(s) for the new Poco if keys are manually generated.
            /// </summary>
            GenIfMnl,

            /// <summary>
            /// Leave key(s) in their default state for new Poco
            /// </summary>
            Default
        }
    }
}
