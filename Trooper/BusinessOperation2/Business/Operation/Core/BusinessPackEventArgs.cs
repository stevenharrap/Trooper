using Trooper.Interface.BusinessOperation2.Business.Operation.Core;

namespace Trooper.BusinessOperation2.Business.Operation.Core
{
    using Autofac;
    using System;

	public class BusinessPackEventArgs : EventArgs, IBusinessPackEventArgs
    {
        public IComponentContext Container { get; set; }
    }
}
