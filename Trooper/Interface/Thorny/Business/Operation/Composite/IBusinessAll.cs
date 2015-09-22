//--------------------------------------------------------------------------------------
// <copyright file="IBusinessCr.cs" company="Trooper Inc">
//     Copyright (c) Trooper 2014 - Onwards
// </copyright>
//--------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ServiceModel;
using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Operation.Single;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Interface.Thorny.Business.Operation.Composite
{
    [ServiceContract]
	public interface IBusinessAll<TPoco> : 
        IBusinessCreate<TPoco>, 
        IBusinessDelete<TPoco>, 
        IBusinessRead<TPoco>, 
        IBusinessRequest<TPoco>,
        IBusinessSession,
        IBusinessUpdate<TPoco>,
        IBusinessSave<TPoco>
        where TPoco : class
    {
    }
}