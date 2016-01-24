namespace Trooper.Thorny.Business.TestSuit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Trooper.Interface.Thorny.Business.Operation.Single;
    using Trooper.Interface.Thorny.Business.Response;
    using Trooper.Interface.Thorny.Business.Security;
    using Security;

    public abstract class TestSuitHelper<TPoco> : TestSuitHelper
        where TPoco : class, new()
    {
        private int counter = 0;
        private int? defaultRequiredValidItems = null;        

        public TestSuitHelper(IBusinessCreate<TPoco> boCreater, IBusinessRead<TPoco> boReader, IBusinessDelete<TPoco> boDeleter)
        {
            this.BoCreater = boCreater;
            this.BoReader = boReader;
            this.BoDeleter = boDeleter;
        }

        #region properties

        public IBusinessCreate<TPoco> BoCreater { get; private set; }

        public IBusinessRead<TPoco> BoReader { get; private set; }

        public IBusinessDelete<TPoco> BoDeleter { get; private set; }

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

        /// <summary>
        /// [Tested]
        /// </summary>
        public IEnumerable<TPoco> MakeInvalidItems(bool incNull, Keys keyBehavious, IEnumerable<TPoco> otherItems)
        {
            var items = this.MakeValidItems(keyBehavious, otherItems).ToList();

            if (incNull)
            {
                items.Add(null);
            }

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
            var response = BoCreater.AddSome(validItems, admin);

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
            var response = BoCreater.Add(validItem, admin);

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
            var response = BoReader.GetByKey(exitingItem, admin);

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
            var response = BoReader.ExistsByKey(validItem, admin);
            
            if (!this.ResponseIsOk(response)) throw new Exception($"{nameof(ItemExists)} could not look for the item.");

            return response.Item;
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public void RemoveAllItems()
        {
            var admin = this.GetAdminIdentity();
            var allResponse = BoReader.GetAll(admin);

            if (!this.ResponseIsOk(allResponse)) throw new Exception($"{nameof(RemoveAllItems)} could not look for all items to remove.");
            if (allResponse.Items == null) throw new Exception($"{nameof(RemoveAllItems)} was not returned any items.");

            var deleteResponse = BoDeleter.DeleteSomeByKey(allResponse.Items, admin);

            if (!this.ResponseIsOk(deleteResponse)) throw new Exception($"{nameof(RemoveAllItems)} could not delete all items.");
        }

        /// <summary>
        /// [Tested]
        /// </summary>
        public IList<TPoco> GetAllItems()
        {
            var admin = this.GetAdminIdentity();
            var allResponse = BoReader.GetAll(admin);

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