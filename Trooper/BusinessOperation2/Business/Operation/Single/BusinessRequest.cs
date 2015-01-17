//--------------------------------------------------------------------------------------
// <copyright file="BusinessRequest.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation2.Business.Operation.Single
{
    using Trooper.BusinessOperation2.Interface.Business.Operation;
    using Trooper.BusinessOperation2.Interface.Business.Operation.Core;
    using Trooper.BusinessOperation2.Interface.Business.Operation.Single;
    using Trooper.BusinessOperation2.Interface.Business.Response;
    using Trooper.BusinessOperation2.Interface.Business.Security;

    public class BusinessRequest<Tc, Ti> : IBusinessRequest<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        public ICredential DefaultCredential { get; set; }

        public IBusinessCore<Tc, Ti> BusinessCore { get; set; }

        public ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, ICredential credential = null)
        {
            return this.BusinessCore.IsAllowed(argument, credential ?? this.DefaultCredential);
        }
    }
}