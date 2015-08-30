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

        public abstract IIdentity MakeValidIdentity();

        public abstract IIdentity MakeInvalidIdentity();

        public bool ItemExists(TPoco item, IBusinessRead<TPoco> boReader)
        {
            var response = boReader.ExistsByKey(item, this.MakeValidIdentity());

            this.CheckResponseForErrors(response);

            return response.Item;
        }

        public void RemoveAllItems(IBusinessRead<TPoco> boReader, IBusinessDelete<TPoco> boDeleter)
        {
            var allResponse = boReader.GetAll(this.MakeValidIdentity());

            this.CheckResponseForErrors(allResponse);

            Assert.IsNotNull(allResponse.Items);

            var deleteResponse = boDeleter.DeleteSomeByKey(allResponse.Items, this.MakeValidIdentity());

            this.CheckResponseForErrors(deleteResponse);
        }

        public void NoItemsExist(IBusinessRead<TPoco> boReader)
        {
            var response = boReader.GetAll(this.MakeValidIdentity());
            this.CheckResponseForErrors(response);
            Assert.That(!response.Items.Any());
        }

        public abstract bool IdentifierAsEqual(TPoco itemA, TPoco itemB);

        public abstract bool NonIdentifersAsEqual(TPoco itemA, TPoco itemB);

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
    }
}
