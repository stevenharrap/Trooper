using Trooper.Interface.BusinessOperation2.Business.Security;

namespace Trooper.Interface.BusinessOperation2.Business.Operation.Core
{
	public interface IBusinessOperation<Tc, Ti>
        where Tc : class, Ti, new()
        where Ti : class
    {
        IIdentity DefaultIdentity { get; set; }

        IBusinessCore<Tc, Ti> BusinessCore { get; set; }
    }
}
