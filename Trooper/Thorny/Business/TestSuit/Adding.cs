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

    public class Adding<TPoco> : IAdding
        where TPoco : class
    {
        private readonly ITestSuitHelper<TPoco> helper;

        private readonly IBusinessCreate<TPoco> boCreat;

        private readonly IBusinessRead<TPoco> boReader;
        
        protected Adding(ITestSuitHelper<TPoco> helper, IBusinessCreate<TPoco> boCreat, IBusinessRead<TPoco> boReader)
        {
            this.helper = helper;
            this.boCreat = boCreat;
            this.boReader = boReader;
        }

        public void DoesAddWhenItemIsValidAndItemDoesNotExistAndIdentityIsAllowed()
        {
            var item = this.helper.MakeValidItem();
            var identity = this.helper.MakeValidIdentity();
            var response = this.boCreat.Add(item, identity);
            
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Ok);
            Assert.IsNotNull(response.Item);
            Assert.That(this.helper.NonIdentifersAsEqual(item, response.Item));
            Assert.IsTrue(this.helper.ItemExists(response.Item, this.boReader));
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
