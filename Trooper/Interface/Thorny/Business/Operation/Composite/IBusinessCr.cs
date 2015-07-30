using System.ServiceModel;
using Trooper.Interface.Thorny.Business.Operation.Single;

namespace Trooper.Interface.Thorny.Business.Operation.Composite
{
    [ServiceContract]
	public interface IBusinessCr<TEnt, TPoco> : IBusinessCreate<TEnt, TPoco>, IBusinessRead<TEnt, TPoco>  
        where TEnt : class, TPoco, new()
        where TPoco : class
    {
    }
}
