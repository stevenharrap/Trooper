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

        public TPoco AddItem(TPoco validItem, IIdentity validIdentity, IBusinessCreate<TPoco> boCreater, IBusinessRead<TPoco> boReader)
        {
            var response = boCreater.Add(validItem, validIdentity);
            this.CheckResponseForErrors(response);

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
            this.CheckResponseForErrors(response);

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

            this.CheckResponseForErrors(response);

            return response.Item;
        }

        public bool ItemExists(TPoco validItem, IBusinessRead<TPoco> boReader)
        {
            return ItemExists(validItem, this.MakeValidIdentity(), boReader);
        }

        public void RemoveAllItems(IIdentity validIdentity, IBusinessRead<TPoco> boReader, IBusinessDelete<TPoco> boDeleter)
        {
            var allResponse = boReader.GetAll(validIdentity);

            this.CheckResponseForErrors(allResponse);

            Assert.IsNotNull(allResponse.Items);

            var deleteResponse = boDeleter.DeleteSomeByKey(allResponse.Items, validIdentity);

            this.CheckResponseForErrors(deleteResponse);
        }

        public void RemoveAllItems(IBusinessRead<TPoco> boReader, IBusinessDelete<TPoco> boDeleter)
        {
            this.RemoveAllItems(this.MakeValidIdentity(), boReader, boDeleter);
        }

        public IList<TPoco> GetAllItems(IIdentity validIdentity, IBusinessRead<TPoco> boReader)
        {
            var allResponse = boReader.GetAll(validIdentity);

            this.CheckResponseForErrors(allResponse);

            Assert.IsNotNull(allResponse.Items);

            return allResponse.Items;
        }

        public IList<TPoco> GetAllItems(IBusinessRead<TPoco> boReader)
        {
            return this.GetAllItems(this.MakeValidIdentity(), boReader);
        }

        public void NoItemsExist(IIdentity validIdentity, IBusinessRead<TPoco> boReader)
        {
            var response = boReader.GetAll(validIdentity);
            this.CheckResponseForErrors(response);
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

        public void CheckResponseForErrors(IResponse response)
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
            NonIdentifiersAreDifferentWhenChanged();
            AnItemIsNewAndIdenticalWhenCopied();
        }

        private void NonIdentifiersAreDifferentWhenChanged()
        {
            var item1 = this.MakeValidItem();
            var item2 = this.CopyItem(item1);

            this.ChangeNonIdentifiers(item2);

            Assert.That(this.IdentifierAreEqual(item1, item2));
            Assert.That(!this.NonIdentifersAreEqual(item1, item2));
        }

        private void AnItemIsNewAndIdenticalWhenCopied()
        {
            var item1 = this.MakeValidItem();
            var item2 = this.CopyItem(item1);

            Assert.That(this.AreEqual(item1, item2));
            Assert.That(!Object.ReferenceEquals(item1, item2));
        }
    }
}
