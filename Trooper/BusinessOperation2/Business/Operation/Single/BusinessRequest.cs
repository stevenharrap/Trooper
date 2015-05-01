//--------------------------------------------------------------------------------------
// <copyright file="BusinessRequest.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation2.Business.Operation.Single
{
	using Interface.Business.Operation.Core;
	using Interface.Business.Operation.Single;
	using Interface.Business.Response;
	using Interface.Business.Security;

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