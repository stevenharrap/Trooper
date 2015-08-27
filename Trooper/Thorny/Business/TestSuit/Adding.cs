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
        public ITestSuitHelper<TPoco> Helper { get; set; }

        public IBusinessCreate<TPoco> Creater { get; set; }

        public IBusinessRead<TPoco> Reader { get; set; }

        public IBusinessDelete<TPoco> Deleter { get; set; }

        [Test]
        public virtual void DoesAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsAllowed()
        {
            var item = this.Helper.MakeValidItem();
            var identity = this.Helper.MakeValidIdentity();
            var response = this.Creater.Add(item, identity);

            this.Helper.RemoveAllItems(this.Reader, this.Deleter);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Ok);
            Assert.IsNotNull(response.Item);
            Assert.That(this.Helper.NonIdentifersAsEqual(item, response.Item));
            Assert.IsTrue(this.Helper.ItemExists(response.Item, this.Reader));
        }

        public virtual void DoesNotAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsNotAllowed()
        {
            throw new NotImplementedException();
        }

        public virtual void DoesNotAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsNull()
        {
            throw new NotImplementedException();
        }

        public virtual void DoesNotAddWhenItemIsInvalidAndIdentityIsAllowed()
        {
            throw new NotImplementedException();
        }

        public virtual void DoesNotAddWhenItemIsInvalidAndIdentityIsNotAllowed()
        {
            throw new NotImplementedException();
        }

        public virtual void DoesNotAddWhenItemIsInvalidAndIdentityIsNull()
        {
            throw new NotImplementedException();
        }

        public virtual void DoesNotAddWhenItemIsNullAndIdentityIsAllowed()
        {
            throw new NotImplementedException();
        }

        public virtual void DoesNotAddWhenItemIsNullAndIdentityIsNotAllowed()
        {
            throw new NotImplementedException();
        }

        public virtual void DoesNotAddWhenItemIsNullAndIdentityIsNull()
        {
            throw new NotImplementedException();
        }

        public virtual void DoesNotAddWhenItemAlreadyExistsAndIdentityIsAllowed()
        {
            throw new NotImplementedException();
        }

        public virtual void DoesNotAddWhenItemAlreadyExistsAndIdentityIsNotAllowed()
        {
            throw new NotImplementedException();
        }

        public virtual void DoesNotAddWhenItemAlreadyExistslAndIdentityIsNull()
        {
            throw new NotImplementedException();
        }
    }
}
