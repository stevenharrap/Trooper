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
    using Trooper.BusinessOperation2.Interface.OperationResponse;

    public class BusinessValidate<Tc, Ti> : IBusinessValidate<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        public ICredential DefaultCredential { get; set; }

        public IBusinessCore<Tc, Ti> BusinessCore { get; set; }

        public IResponse Validate(Ti item, ICredential credential = null)
        {
            return this.Validate(item, credential ?? this.DefaultCredential);
        }

        public ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, ICredential credential = null)
        {
            return this.IsAllowed(argument, credential ?? this.DefaultCredential);
        }
    }
}