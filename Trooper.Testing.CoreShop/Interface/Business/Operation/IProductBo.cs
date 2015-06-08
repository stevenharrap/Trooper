using System.ServiceModel;
using Trooper.Interface.Thorny.Business.Operation.Composite;
using Trooper.Interface.Thorny.Business.Operation.Single;
using Trooper.Testing.ShopModel;
using Trooper.Testing.ShopModel.Interface;
using Trooper.Testing.ShopModel.Model;

namespace Trooper.Testing.CustomShopApi.Interface.Business.Operation
{
    [ServiceContract]
    public interface IProductBo : IBusinessRead<Product, IProduct>
    {
    }
}
