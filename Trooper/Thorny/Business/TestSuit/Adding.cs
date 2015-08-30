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
    using Trooper.Thorny.Business.Operation.Core;

    public abstract class Adding<TPoco> : IAdding
        where TPoco : class
    {
        public ITestSuitHelper<TPoco> Helper { get; set; }

        public IBusinessCreate<TPoco> Creater { get; set; }

        public IBusinessRead<TPoco> Reader { get; set; }

        public IBusinessDelete<TPoco> Deleter { get; set; }

        /// <summary>
        ///     Response.Item = added item
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        [Test]
        public virtual void DoesAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsAllowed()
        {
            var item = this.Helper.MakeValidItem();
            var identity = this.Helper.MakeValidIdentity();            

            this.Helper.RemoveAllItems(this.Reader, this.Deleter);
            var response = this.Creater.Add(item, identity);
            this.Helper.CheckResponseForErrors(response);

            Assert.IsNotNull(response.Item);
            Assert.That(this.Helper.NonIdentifersAsEqual(item, response.Item));
            Assert.That(!this.Helper.IdentifierAsEqual(item, response.Item));
        }

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Access Denied]
        /// </summary>
        [Test]
        public virtual void DoesNotAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsNotAllowed()
        {
            var item = this.Helper.MakeValidItem();
            var identity = this.Helper.MakeInvalidIdentity();

            this.Helper.RemoveAllItems(this.Reader, this.Deleter);
            var response = this.Creater.Add(item, identity);

            Assert.IsNull(response.Item);
            this.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode);
            this.Helper.NoItemsExist(this.Reader);
        }

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        [Test]
        public virtual void DoesNotAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsNull()
        {
            var item = this.Helper.MakeValidItem();

            this.Helper.RemoveAllItems(this.Reader, this.Deleter);
            var response = this.Creater.Add(item, null);

            Assert.IsNull(response.Item);
            this.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode);
            this.Helper.NoItemsExist(this.Reader);
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
