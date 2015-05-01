//--------------------------------------------------------------------------------------
// <copyright file="BusinessR.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using Trooper.Interface.BusinessOperation2.Business.Operation.Core;
using Trooper.Interface.BusinessOperation2.Business.Operation.Single;
using Trooper.Interface.BusinessOperation2.Business.Response;
using Trooper.Interface.BusinessOperation2.Business.Security;

namespace Trooper.BusinessOperation2.Business.Operation.Single
{
	using Interface.DataManager;

    public class BusinessRead<Tc, Ti> : IBusinessRead<Tc, Ti> 
        where Tc : class, Ti, new()
        where Ti : class
    {
        public IIdentity DefaultIdentity { get; set; }

        public IBusinessCore<Tc, Ti> BusinessCore { get; set; }

        public IManyResponse<Ti> GetAll(IIdentity identity = null)
        {
            return this.BusinessCore.GetAll(identity ?? this.DefaultIdentity);
        }

        public IManyResponse<Ti> GetSome(ISearch search, IIdentity identity = null)
        {
            return this.BusinessCore.GetSome(search, identity ?? this.DefaultIdentity);
        }

        public ISingleResponse<Ti> GetByKey(Ti item, IIdentity identity = null)
        {
            return this.BusinessCore.GetByKey(item, identity ?? this.DefaultIdentity);
        }

        public ISingleResponse<bool> ExistsByKey(Ti item, IIdentity identity = null)
        {
			return this.BusinessCore.ExistsByKey(item, identity ?? this.DefaultIdentity);
        }

        public ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, IIdentity identity = null)
        {
			return this.BusinessCore.IsAllowed(argument, identity ?? this.DefaultIdentity);
        }
    }
}