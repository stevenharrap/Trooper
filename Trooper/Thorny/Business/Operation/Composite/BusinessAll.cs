//--------------------------------------------------------------------------------------
// <copyright file="BusinessCr.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using Trooper.Interface.Thorny.Business.Operation.Composite;
using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Business.Operation.Composite
{
    using System;
    using System.Collections.Generic;
    using Trooper.Interface.Thorny.Business.Operation.Single;

    public class BusinessAll<TEnt, TPoco> : IBusinessAll<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public IBusinessCore<TEnt, TPoco> BusinessCore { get; set; }

        public IAddResponse<TPoco> Add(TPoco item, IIdentity identity)
        {
            return this.BusinessCore.Add(item, identity);
        }

        public IAddSomeResponse<TPoco> AddSome(IEnumerable<TPoco> items, IIdentity identity)
        {
            return this.BusinessCore.AddSome(items, identity);
        }

        public ISingleResponse<bool> IsAllowed(IRequestArg<TPoco> argument, IIdentity identity)
        {
            return this.BusinessCore.IsAllowed(argument, identity);
        }

        public ISingleResponse<Guid> GetSession(IIdentity identity)
        {
            return this.BusinessCore.GetSession(identity);
        }

        public IManyResponse<TPoco> GetAll(IIdentity identity)
        {
            return this.BusinessCore.GetAll(identity);
        }

        public IManyResponse<TPoco> GetSome(Interface.DataManager.ISearch search, IIdentity identity)
        {
            return this.BusinessCore.GetSome(search, identity);
        }

        public ISingleResponse<TPoco> GetByKey(TPoco item, IIdentity identity)
        {
            return this.BusinessCore.GetByKey(item, identity);
        }

		public IManyResponse<TPoco> GetSomeByKey(IEnumerable<TPoco> items, IIdentity identity)
		{
			return this.BusinessCore.GetSomeByKey(items, identity);
		}

        public ISingleResponse<bool> ExistsByKey(TPoco item, IIdentity identity)
        {
            return this.BusinessCore.ExistsByKey(item, identity);
        }

        public IResponse DeleteByKey(TPoco item, IIdentity identity)
        {
            return this.BusinessCore.DeleteByKey(item, identity);
        }

        public IResponse DeleteSomeByKey(IEnumerable<TPoco> items, IIdentity identity)
        {
            return this.BusinessCore.DeleteSomeByKey(items, identity);
        }

        public ISingleResponse<TPoco> Update(TPoco item, IIdentity identity)
        {
            return this.BusinessCore.Update(item, identity);
        }

		public IManyResponse<TPoco> UpdateSome(IEnumerable<TPoco> items, IIdentity identity)
		{
			return this.BusinessCore.UpdateSome(items, identity);
		}

        public ISaveResponse<TPoco> Save(TPoco item, IIdentity identity)
        {
            return this.BusinessCore.Save(item, identity);
        }

		public IManyResponse<TPoco> SaveSome(IEnumerable<TPoco> items, IIdentity identity)
		{
			return this.BusinessCore.UpdateSome(items, identity);
		}
    }
}