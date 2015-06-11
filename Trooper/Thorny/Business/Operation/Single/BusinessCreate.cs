//--------------------------------------------------------------------------------------
// <copyright file="BusinessCr.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Operation.Single;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Business.Operation.Single
{
    using System.Collections.Generic;
    using Trooper.Thorny.Interface.OperationResponse;

    public class BusinessCreate<Tc, Ti> : IBusinessCreate<Tc, Ti> 
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
    }
}