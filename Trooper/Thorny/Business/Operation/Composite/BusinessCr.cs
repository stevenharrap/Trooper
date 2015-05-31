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
    using System.Collections.Generic;
    using Trooper.Thorny.Interface.OperationResponse;

    public class BusinessCr<Tc, Ti> : IBusinessCr<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {

        public IBusinessCore<Tc, Ti> BusinessCore { get; set; }

        public IAddResponse<Ti> Add(Ti item, IIdentity identity)
        {
            return this.BusinessCore.Add(item, identity);
        }

        public IAddSomeResponse<Ti> AddSome(IEnumerable<Ti> items, IIdentity identity)
        {
            return this.BusinessCore.AddSome(items, identity);
        }

        public ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, IIdentity identity)
        {
            return this.BusinessCore.IsAllowed(argument, identity);
        }

        public IManyResponse<Ti> GetAll(IIdentity identity)
        {
            return this.BusinessCore.GetAll(identity);
        }

        public IManyResponse<Ti> GetSome(Interface.DataManager.ISearch search, IIdentity identity)
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
    }
}