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
    using Trooper.Thorny.Interface.OperationResponse;

    public class BusinessValidate<Tc, Ti> : IBusinessValidate<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        public IIdentity DefaultIdentity { get; set; }

        public IBusinessCore<Tc, Ti> BusinessCore { get; set; }

        public IResponse Validate(Ti item, IIdentity identity = null)
        {
            return this.Validate(item, identity ?? this.DefaultIdentity);
        }

        public ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, IIdentity identity = null)
        {
            return this.IsAllowed(argument, identity ?? this.DefaultIdentity);
        }
    }
}