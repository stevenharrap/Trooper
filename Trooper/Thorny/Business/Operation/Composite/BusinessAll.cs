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
    using Trooper.Thorny.Interface.OperationResponse;
    using Trooper.Interface.Thorny.Business.Operation.Single;

    public class BusinessAll<Tc, Ti> : IBusinessAll<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        public IIdentity DefaultIdentity { get; set; }

        public IBusinessCore<Tc, Ti> BusinessCore { get; set; }

        public IAddResponse<Ti> Add(Ti item, IIdentity identity = null)
        {
            return this.BusinessCore.Add(item, identity ?? this.DefaultIdentity);
        }

        public IAddSomeResponse<Ti> AddSome(IEnumerable<Ti> items, IIdentity identity = null)
        {
            return this.BusinessCore.AddSome(items, identity ?? this.DefaultIdentity);
        }

        public ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, IIdentity identity = null)
        {
            return this.BusinessCore.IsAllowed(argument, identity ?? this.DefaultIdentity);
        }

        public ISingleResponse<Guid> GetSession(IIdentity identity = null)
        {
            return this.BusinessCore.GetSession(identity ?? this.DefaultIdentity);
        }

        public IManyResponse<Ti> GetAll(IIdentity identity = null)
        {
            return this.BusinessCore.GetAll(identity ?? this.DefaultIdentity);
        }

        public IManyResponse<Ti> GetSome(Interface.DataManager.ISearch search, IIdentity identity = null)
        {
            return this.BusinessCore.GetSome(search, identity ?? this.DefaultIdentity);
        }

        public ISingleResponse<Ti> GetByKey(Ti item, IIdentity identity = null)
        {
            return this.BusinessCore.GetByKey(item, identity ?? this.DefaultIdentity);
        }

		public IManyResponse<Ti> GetSomeByKey(IEnumerable<Ti> items, IIdentity identity = null)
		{
			return this.BusinessCore.GetSomeByKey(items, identity ?? this.DefaultIdentity);
		}

        public ISingleResponse<bool> ExistsByKey(Ti item, IIdentity identity = null)
        {
            return this.BusinessCore.ExistsByKey(item, identity ?? this.DefaultIdentity);
        }

        public IResponse DeleteByKey(Ti item, IIdentity identity = null)
        {
            return this.BusinessCore.DeleteByKey(item, identity ?? this.DefaultIdentity);
        }

        public IResponse DeleteSomeByKey(IEnumerable<Ti> items, IIdentity identity = null)
        {
            return this.BusinessCore.DeleteSomeByKey(items, identity ?? this.DefaultIdentity);
        }

        public ISingleResponse<Ti> Update(Ti item, IIdentity identity = null)
        {
            return this.BusinessCore.Update(item, identity ?? this.DefaultIdentity);
        }

		public IManyResponse<Ti> UpdateSome(IEnumerable<Ti> items, IIdentity identity = null)
		{
			return this.BusinessCore.UpdateSome(items, identity ?? this.DefaultIdentity);
		}

        public ISaveResponse<Ti> Save(Ti item, IIdentity identity = null)
        {
            return this.BusinessCore.Save(item, identity ?? this.DefaultIdentity);
        }

		public IManyResponse<Ti> SaveSome(IEnumerable<Ti> items, IIdentity identity = null)
		{
			return this.BusinessCore.UpdateSome(items, identity ?? this.DefaultIdentity);
		}
    }
}