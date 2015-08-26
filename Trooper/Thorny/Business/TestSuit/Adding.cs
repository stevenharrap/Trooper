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
        public abstract ITestSuitHelper<TPoco> Helper { get; set; }

        public abstract IBusinessCreate<TPoco> Creater { get; set; }

        public abstract IBusinessRead<TPoco> Reader { get; set; } 

        [Test]
        public virtual void DoesAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsAllowed()
        {
            var helper = this.Helper;
            var creater = this.Creater;
            var reader = this.Reader;

            var item = helper.MakeValidItem();
            var identity = helper.MakeValidIdentity();
            var response = creater.Add(item, identity);
            
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Ok);
            Assert.IsNotNull(response.Item);
            Assert.That(helper.NonIdentifersAsEqual(item, response.Item));
            Assert.IsTrue(helper.ItemExists(response.Item, reader));
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
