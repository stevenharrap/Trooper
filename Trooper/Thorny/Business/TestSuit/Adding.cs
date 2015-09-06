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

    public class x<TPoco> : IDisposable
        where TPoco : class
    {
        public ITestSuitHelper<TPoco> Helper { get; private set; }

        public IBusinessCreate<TPoco> Creater { get; private set; }

        public IBusinessRead<TPoco> Reader { get; private set; }

        public IBusinessDelete<TPoco> Deleter { get; private set; }

        public x(ITestSuitHelper<TPoco> helper, IBusinessCreate<TPoco> creater, IBusinessRead<TPoco> reader, IBusinessDelete<TPoco> deleter)
        {
            this.Helper = helper;
            this.Creater = creater;
            this.Reader = reader;
            this.Deleter = deleter;
        }

        public void Dispose()
        {
            this.Helper = null;
            this.Creater = null;
            this.Reader = null;
            this.Deleter = null;
        }
    }

    public abstract class Adding<TPoco> : IAdding
        where TPoco : class
    {
        public abstract Func<x<TPoco>> XMaker { get; }

        //public ITestSuitHelper<TPoco> Helper { get; set; }

        //public IBusinessCreate<TPoco> Creater { get; set; }

        //public IBusinessRead<TPoco> Reader { get; set; }

        //public IBusinessDelete<TPoco> Deleter { get; set; }

        /// <summary>
        ///     Response.Item = added item
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        [Test]
        public virtual void DoesAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsAllowed()
        {
            using (var x1 = this.XMaker())
            {
                var item = x1.Helper.MakeValidItem();
                var identity = x1.Helper.MakeValidIdentity();

                x1.Helper.RemoveAllItems(x1.Reader, x1.Deleter);
                var response = x1.Creater.Add(item, identity);
                x1.Helper.CheckResponseForErrors(response);

                Assert.IsNotNull(response.Item);
                Assert.That(x1.Helper.NonIdentifersAsEqual(item, response.Item));
                Assert.That(!x1.Helper.IdentifierAsEqual(item, response.Item));
            }
        }

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Access Denied]
        /// </summary>
        [Test]
        public virtual void DoesNotAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsNotAllowed()
        {
            using (var x1 = this.XMaker())
            {
                var item = x1.Helper.MakeValidItem();
                var identity = x1.Helper.MakeInvalidIdentity();

                x1.Helper.RemoveAllItems(x1.Reader, x1.Deleter);
                var response = x1.Creater.Add(item, identity);

                Assert.IsNull(response.Item);
                x1.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode);
                x1.Helper.NoItemsExist(x1.Reader);
            }
        }

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        [Test]
        public virtual void DoesNotAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsNull()
        {
            using (var x1 = this.XMaker())
            {
                var item = x1.Helper.MakeValidItem();

                x1.Helper.RemoveAllItems(x1.Reader, x1.Deleter);
                var response = x1.Creater.Add(item, null);

                Assert.IsNull(response.Item);
                x1.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode);
                x1.Helper.NoItemsExist(x1.Reader);
            }
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
