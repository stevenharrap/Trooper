using System.ServiceModel;
using Trooper.Interface.Thorny.Business.Operation.Single;

namespace Trooper.Interface.Thorny.Business.Operation.Composite
{
    [ServiceContract]
	public interface IBusinessCr<TPoco> : IBusinessCreate<TPoco>, IBusinessRead<TPoco>  
        where TPoco : class
    {
    }
}
