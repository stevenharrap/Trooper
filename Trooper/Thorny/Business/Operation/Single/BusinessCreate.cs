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

    public class BusinessCreate<TEnt, TPoco> : IBusinessCreate<TEnt, TPoco> 
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
    }
}