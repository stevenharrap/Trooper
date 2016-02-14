namespace Trooper.Testing.CustomShop.Api.Interface.Business.Operation
{
    using System.ServiceModel;
    using ShopPoco.View;
    using Trooper.Interface.Thorny.Business.Operation.Single;

    [ServiceContract]
    public interface IOutletViewBo : IBusinessRead<OutletView>
    {
    }
}
