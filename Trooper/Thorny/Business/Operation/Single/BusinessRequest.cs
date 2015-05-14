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
	public class BusinessRequest<Tc, Ti> : IBusinessRequest<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        public IIdentity DefaultIdentity { get; set; }

        public IBusinessCore<Tc, Ti> BusinessCore { get; set; }

        public ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, IIdentity identity = null)
        {
            return this.BusinessCore.IsAllowed(argument, identity ?? this.DefaultIdentity);
        }
    }
}