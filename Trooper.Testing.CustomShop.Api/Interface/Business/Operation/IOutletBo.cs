namespace Trooper.Testing.CustomShop.Api.Interface.Business.Operation
{
    using System.ServiceModel;
    using Trooper.Interface.Thorny.Business.Operation.Composite;
    using ShopPoco;

    [ServiceContract]
    public interface IOutletBo : IBusinessAll<Outlet>
    {
        //[OperationContract]
        //ISaveResponse<ProductInOutlet> SaveProduct(ProductInOutlet productInShop, IIdentity identity);

        //[OperationContract]
        //IAddResponse<Outlet> SimpleLittleThing(IIdentity identity);
    }
}
