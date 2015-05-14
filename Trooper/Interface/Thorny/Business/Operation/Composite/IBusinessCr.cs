using Trooper.Interface.Thorny.Business.Operation.Single;

namespace Trooper.Interface.Thorny.Business.Operation.Composite
{
	public interface IBusinessCr<Tc, Ti> : IBusinessCreate<Tc, Ti>, IBusinessRead<Tc, Ti>  
        where Tc : class, Ti, new()
        where Ti : class
    {
    }
}
