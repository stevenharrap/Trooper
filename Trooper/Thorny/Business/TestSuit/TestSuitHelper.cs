using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trooper.Interface.Thorny.Business.Operation.Single;
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
            var response = boReader.ExistsByKey(item, this.MakeInvalidIdentity());

            Assert.IsTrue(response.Ok);
            return response.Item;
        }

        public void RemoveAllItems(IBusinessRead<TPoco> boReader, IBusinessDelete<TPoco> boDeleter)
        {
            var allResponse = boReader.GetAll(this.MakeValidIdentity());

            Assert.IsTrue(allResponse.Ok);
            Assert.IsNotNull(allResponse.Items);

            var deleteResponse = boDeleter.DeleteSomeByKey(allResponse.Items, this.MakeValidIdentity());

            Assert.IsTrue(deleteResponse.Ok);
        }

        public abstract bool IdentifierAsEqual(TPoco itemA, TPoco itemB);

        public abstract bool NonIdentifersAsEqual(TPoco itemA, TPoco itemB);
    }
}
