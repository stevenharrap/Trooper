using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Trooper.Thorny.Business.TestSuit.Adding
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
        public abstract Func<AddingRequirment<TPoco>> Requirement { get; }

        /// <summary>
        ///     Response.Item = added item
        ///     Response.Ok = true
        ///     Response.Messages = empty
        /// </summary>
        [Test]
        public virtual void DoesAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsAllowed()
        {
            using (var recquirment = this.Requirement())
            {
                var item = recquirment.Helper.MakeValidItem();
                var identity = recquirment.Helper.MakeValidIdentity();

                recquirment.Helper.RemoveAllItems(recquirment.Reader, recquirment.Deleter);
                var response = recquirment.Creater.Add(item, identity);
                recquirment.Helper.CheckResponseForErrors(response);

                Assert.IsNotNull(response.Item);
                Assert.That(recquirment.Helper.NonIdentifersAsEqual(item, response.Item));
                Assert.That(!recquirment.Helper.IdentifierAsEqual(item, response.Item));
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
            using (var requirment = this.Requirement())
            {
                requirment.Helper.RemoveAllItems(requirment.Reader, requirment.Deleter);

                var item = requirment.Helper.MakeValidItem();
                var identity = requirment.Helper.MakeInvalidIdentity();                
                var response = requirment.Creater.Add(item, identity);

                Assert.IsNull(response.Item);
                requirment.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode);
                requirment.Helper.NoItemsExist(requirment.Reader);
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
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems(requirement.Reader, requirement.Deleter);

                var item = requirement.Helper.MakeValidItem();                
                var response = requirement.Creater.Add(item, null);
                                
                Assert.IsNull(response.Item);
                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode);
                requirement.Helper.NoItemsExist(requirement.Reader);
            }
        }

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Invalid item]
        /// </summary>
        [Test]
        public virtual void DoesNotAddWhenItemIsInvalidAndIdentityIsAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems(requirement.Reader, requirement.Deleter);

                var item = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeValidIdentity();
                var response = requirement.Creater.Add(item, identity);

                Assert.IsNull(response.Item);
                requirement.Helper.ResponseFailsWithError(response, BusinessCore.InvalidPropertyCode);
                requirement.Helper.NoItemsExist(requirement.Reader);
            }
        }

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Access Denied]
        /// </summary>
        [Test]
        public virtual void DoesNotAddWhenItemIsInvalidAndIdentityIsNotAllowed()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems(requirement.Reader, requirement.Deleter);

                var item = requirement.Helper.MakeInvalidItem();
                var identity = requirement.Helper.MakeInvalidIdentity();
                var response = requirement.Creater.Add(item, identity);

                Assert.IsNull(response.Item);
                requirement.Helper.ResponseFailsWithError(response, BusinessCore.UserDeniedCode);
                requirement.Helper.NoItemsExist(requirement.Reader);
            }
        }

        /// <summary>
        ///     Response.Item = null
        ///     Response.Ok = false
        ///     Response.Messages = [Identity not supplied]
        /// </summary>
        [Test]
        public virtual void DoesNotAddWhenItemIsInvalidAndIdentityIsNull()
        {
            using (var requirement = this.Requirement())
            {
                requirement.Helper.RemoveAllItems(requirement.Reader, requirement.Deleter);

                var item = requirement.Helper.MakeInvalidItem();
                var response = requirement.Creater.Add(item, null);

                Assert.IsNull(response.Item);
                requirement.Helper.ResponseFailsWithError(response, BusinessCore.NullIdentityCode);
                requirement.Helper.NoItemsExist(requirement.Reader);
            }
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
