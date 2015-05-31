using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Interface.Thorny.Business.Operation.Core
{
	public interface IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        IBusinessCore<Tc, Ti> BusinessCore { get; set; }
    }
}
