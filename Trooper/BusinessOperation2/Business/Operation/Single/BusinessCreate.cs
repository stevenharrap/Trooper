//--------------------------------------------------------------------------------------
// <copyright file="BusinessCr.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

namespace Trooper.BusinessOperation2.Business.Operation.Single
{
    using System.Collections.Generic;
    using Trooper.BusinessOperation2.Interface.Business.Operation;
    using Trooper.BusinessOperation2.Interface.Business.Operation.Core;
    using Trooper.BusinessOperation2.Interface.Business.Operation.Single;
    using Trooper.BusinessOperation2.Interface.Business.Response;
    using Trooper.BusinessOperation2.Interface.Business.Security;
    using Trooper.BusinessOperation2.Interface.OperationResponse;

    public class BusinessCreate<Tc, Ti> : IBusinessCreate<Tc, Ti> 
        where Tc : class, Ti, new()
        where Ti : class
    {
        public ICredential DefaultCredential { get; set; }

        public IBusinessCore<Tc, Ti> BusinessCore { get; set; }

        public IAddResponse<Ti> Add(Ti item, ICredential credential = null)
        {
            return this.BusinessCore.Add(item, credential ?? this.DefaultCredential);
        }

        public IAddSomeResponse<Ti> AddSome(IEnumerable<Ti> items, ICredential credential = null)
        {
            return this.BusinessCore.AddSome(items, credential ?? this.DefaultCredential);
        }

        public IResponse Validate(Ti item, ICredential credential = null)
        {
            return this.BusinessCore.Validate(item, credential ?? this.DefaultCredential);
        }

        public ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, ICredential credential = null)
        {
            return this.BusinessCore.IsAllowed(argument, credential ?? this.DefaultCredential);
        }
    }
}