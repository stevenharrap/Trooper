//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCr.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System.Collections.Generic;
using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Operation.Single;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Interface.Thorny.Business.Operation.Composite
{
	public interface IBusinessAll<Tc, Ti> : 
        IBusinessCreate<Tc, Ti>, 
        IBusinessDelete<Tc, Ti>, 
        IBusinessRead<Tc, Ti>, 
        IBusinessRequest<Tc, Ti>,
        IBusinessSession,
        IBusinessUpdate<Tc, Ti>,
        IBusinessValidate<Tc, Ti>, 
        IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
    }
}