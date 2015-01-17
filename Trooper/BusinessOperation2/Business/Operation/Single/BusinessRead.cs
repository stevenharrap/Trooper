//--------------------------------------------------------------------------------------
// <copyright file="BusinessR.cs" company="Trooper Inc">
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
    using Trooper.BusinessOperation2.Interface.DataManager;

    public class BusinessRead<Tc, Ti> : IBusinessRead<Tc, Ti> 
        where Tc : class, Ti, new()
        where Ti : class
    {
        public ICredential DefaultCredential { get; set; }

        public IBusinessCore<Tc, Ti> BusinessCore { get; set; }

        public IManyResponse<Ti> GetAll(ICredential credential = null)
        {
            return this.BusinessCore.GetAll(credential ?? this.DefaultCredential);
        }

        public IManyResponse<Ti> GetSome(ISearch search, ICredential credential = null)
        {
            return this.BusinessCore.GetSome(search, credential ?? this.DefaultCredential);
        }

        public ISingleResponse<Ti> GetByKey(Ti item, ICredential credential = null)
        {
            return this.BusinessCore.GetByKey(item, credential ?? this.DefaultCredential);
        }

        public ISingleResponse<bool> ExistsByKey(Ti item, ICredential credential = null)
        {
            return this.BusinessCore.ExistsByKey(item, credential ?? this.DefaultCredential);
        }

        public ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, ICredential credential = null)
        {
            return this.BusinessCore.IsAllowed(argument, credential ?? this.DefaultCredential);
        }
    }
}