using Autofac;

namespace Trooper.Interface.Thorny.Business.Operation.Core
{
	public interface IBusinessPackEventArgs
    {
        IComponentContext Container { get; set; }
    }
}
