using Trooper.Interface.Thorny.Business.Operation.Core;

namespace Trooper.Thorny.Business.Operation.Core
{
    using Autofac;
    using System;

	public class BusinessPackEventArgs : EventArgs, IBusinessPackEventArgs
    {
        public IComponentContext Container { get; set; }
    }
}
