//--------------------------------------------------------------------------------------
// <copyright file="BusinessR.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Operation.Single;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Business.Operation.Single
{
	using Interface.DataManager;

    public class BusinessRead<Tc, Ti> : IBusinessRead<Tc, Ti> 
        where Tc : class, Ti, new()
        where Ti : class
    {
        public IBusinessCore<Tc, Ti> BusinessCore { get; set; }

        public IManyResponse<Ti> GetAll(IIdentity identity)
        {
            return this.BusinessCore.GetAll(identity);
        }

        public IManyResponse<Ti> GetSome(ISearch search, IIdentity identity)
        {
            return this.BusinessCore.GetSome(search, identity);
        }

        public ISingleResponse<Ti> GetByKey(Ti item, IIdentity identity)
        {
            return this.BusinessCore.GetByKey(item, identity);
        }

        public ISingleResponse<bool> ExistsByKey(Ti item, IIdentity identity)
        {
			return this.BusinessCore.ExistsByKey(item, identity);
        }

        public ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, IIdentity identity)
        {
			return this.BusinessCore.IsAllowed(argument, identity);
        }
    }
}