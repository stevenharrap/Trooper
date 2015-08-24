using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Thorny.Business.TestSuit
{
    using FluentAssertions;

    using NUnit.Framework;

    using Trooper.Interface.Thorny.Business.Operation.Single;
    using Trooper.Interface.Thorny.TestSuit;
    using Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit;

    public abstract class Adding<TPoco> : IAdding
        where TPoco : class
    {
        public abstract ITestSuitHelper<TPoco> GetHelper();

        public abstract IBusinessCreate<TPoco> GetCreater();

        public abstract IBusinessRead<TPoco> GetReader();

        [Test]
        public void DoesAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsAllowed()
        {
            var helper = this.GetHelper();
            var creater = this.GetCreater();
            var reader = this.GetReader();

            var item = helper.MakeValidItem();
            var identity = helper.MakeValidIdentity();
            var response = creater.Add(item, identity);
            
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Ok);
            Assert.IsNotNull(response.Item);
            Assert.That(helper.NonIdentifersAsEqual(item, response.Item));
            Assert.IsTrue(helper.ItemExists(response.Item, reader));
        }

        public void DoesNotAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsNotAllowed()
        {
            throw new NotImplementedException();
        }

        public void DoesNotAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsNull()
        {
            throw new NotImplementedException();
        }

        public void DoesNotAddWhenItemIsInvalidAndIdentityIsAllowed()
        {
            throw new NotImplementedException();
        }

        public void DoesNotAddWhenItemIsInvalidAndIdentityIsNotAllowed()
        {
            throw new NotImplementedException();
        }

        public void DoesNotAddWhenItemIsInvalidAndIdentityIsNull()
        {
            throw new NotImplementedException();
        }

        public void DoesNotAddWhenItemIsNullAndIdentityIsAllowed()
        {
            throw new NotImplementedException();
        }

        public void DoesNotAddWhenItemIsNullAndIdentityIsNotAllowed()
        {
            throw new NotImplementedException();
        }

        public void DoesNotAddWhenItemIsNullAndIdentityIsNull()
        {
            throw new NotImplementedException();
        }

        public void DoesNotAddWhenItemAlreadyExistsAndIdentityIsAllowed()
        {
            throw new NotImplementedException();
        }

        public void DoesNotAddWhenItemAlreadyExistsAndIdentityIsNotAllowed()
        {
            throw new NotImplementedException();
        }

        public void DoesNotAddWhenItemAlreadyExistslAndIdentityIsNull()
        {
            throw new NotImplementedException();
        }
    }
}
