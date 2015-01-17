namespace Trooper.BusinessOperation2.Interface.Business.Operation.Core
{
    using Autofac;

    public interface IBusinessPackEventArgs
    {
        IComponentContext Container { get; set; }
    }
}
