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
        public IIdentity DefaultIdentity { get; set; }

        public IBusinessCore<Tc, Ti> BusinessCore { get; set; }

        public IManyResponse<Ti> GetAll(IIdentity identity = null)
        {
            return this.BusinessCore.GetAll(identity ?? this.DefaultIdentity);
        }

        public IManyResponse<Ti> GetSome(ISearch search, IIdentity identity = null)
        {
            return this.BusinessCore.GetSome(search, identity ?? this.DefaultIdentity);
        }

        public ISingleResponse<Ti> GetByKey(Ti item, IIdentity identity = null)
        {
            return this.BusinessCore.GetByKey(item, identity ?? this.DefaultIdentity);
        }

        public ISingleResponse<bool> ExistsByKey(Ti item, IIdentity identity = null)
        {
            return this.BusinessCore.ExistsByKey(item, credential ?? this.DefaultIdentity);
        }

        public ISingleResponse<bool> IsAllowed(IRequestArg<Ti> argument, IIdentity identity = null)
        {
            return this.BusinessCore.IsAllowed(argument, credential ?? this.DefaultCredential);
        }
    }
}