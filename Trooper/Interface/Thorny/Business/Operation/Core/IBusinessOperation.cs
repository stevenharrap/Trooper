using Trooper.Interface.Thorny.Business.Security;

namespace Trooper.Interface.Thorny.Business.Operation.Core
{
	public interface IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        IIdentity DefaultIdentity { get; set; }

        IBusinessCore<Tc, Ti> BusinessCore { get; set; }
    }
}
