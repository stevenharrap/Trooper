using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Interface.Thorny.Business.Operation.Core
{
    public interface IBusinessOperation<TEnt, TPoco> : IBusinessOperation
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
        IBusinessCore<TEnt, TPoco> BusinessCore { get; set; }
    }

    public interface IBusinessOperation
    {
    }
}
