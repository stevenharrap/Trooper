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


    public class BusinessCr<TEnt, TPoco> : IBusinessCr<TPoco>, IBusinessOperation<TEnt, TPoco>
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

        public ISingleResponse<bool> ExistsByKey(TPoco item, IIdentity identity)
        {
            return this.BusinessCore.ExistsByKey(item, identity);
        }
    }
}