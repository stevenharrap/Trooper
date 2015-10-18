namespace Trooper.Testing.CustomShop.Api.Interface.Business.Operation
{
    using System.ServiceModel;
    using Trooper.Interface.Thorny.Business.Operation.Single;
    using ShopPoco;

    [ServiceContract]
    public interface IProductBo : IBusinessRead<Product>
    {
    }
}
