namespace Trooper.BusinessOperation2.Business.Operation.Core
{
    using Autofac;
    using System;
    using Trooper.BusinessOperation2.Interface.Business.Operation.Core;

    public class BusinessPackEventArgs : EventArgs, IBusinessPackEventArgs
    {
        public IComponentContext Container { get; set; }
    }
}
