//--------------------------------------------------------------------------------------
// <copyright file="BusinessRequest.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Operation.Single;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Thorny.Business.Operation.Single
{
    public class BusinessRequest<TEnt, TPoco> : IBusinessRequest<TPoco>, IBusinessOperation<TEnt, TPoco>
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        public IBusinessCore<TEnt, TPoco> BusinessCore { get; set; }

        public ISingleResponse<bool> IsAllowed(IRequestArg<TPoco> argument, IIdentity identity)
        {
            return this.BusinessCore.IsAllowed(argument, identity);
        }
    }
}