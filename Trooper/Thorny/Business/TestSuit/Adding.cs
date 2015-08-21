using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trooper.Thorny.Business.TestSuit
{
    using NUnit.Framework;

    using Trooper.Interface.Thorny.TestSuit;
    using Trooper.Interface.Thorny.TestSuit.BusinessCoreTestSuit;

    public abstract class Adding<TItem> : IAdding
    {
        private readonly ITestSuitHelper<TItem> helper;
        
        protected Adding(ITestSuitHelper<TItem> helper)
        {
            this.helper = helper;
        }

        [Test]
        public void DoesAddWhenItemIsValidAndIdentityIsAllowed()
        {
            var item = this.helper.GetValidItem();
            var identity = this.helper.GetValidIdentity();

            



            throw new NotImplementedException();
        }

        public void DoesNotAddWhenItemIsValidAndIdentityIsNotAllowed()
        {
            throw new NotImplementedException();
        }

        public void DoesNotAddWhenItemIsValidAndIdentityIsNull()
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
