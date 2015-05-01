using Autofac;

namespace Trooper.Interface.BusinessOperation2.Business.Operation.Core
{
	public interface IBusinessPackEventArgs
    {
        IComponentContext Container { get; set; }
    }
}
