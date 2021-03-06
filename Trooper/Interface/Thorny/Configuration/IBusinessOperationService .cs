﻿namespace Trooper.Interface.Thorny.Configuration
{
    using Autofac;
    using System;
    using System.ServiceModel;
    using Trooper.Interface.DynamicServiceHost;
    using Trooper.Interface.Thorny.Business.Operation.Core;

    public interface IBusinessOperationService : IStartable
    {
        bool AutoStart { get; set; }

        ServiceHost ServiceHost { get; }

        void Stop();
    }
}
