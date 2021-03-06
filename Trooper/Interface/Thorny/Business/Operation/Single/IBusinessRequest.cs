﻿using System;
using System.ServiceModel;
using Trooper.Interface.Thorny.Business.Operation.Core;
using Trooper.Interface.Thorny.Business.Response;
using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Interface.Thorny.Business.Operation.Single
{
    [ServiceContract]
	public interface IBusinessRequest<TPoco>
        where TPoco : class
    {
        [OperationContract]
        ISingleResponse<bool> IsAllowed(IRequestArg<TPoco> argument, IIdentity identity);
    }
}
