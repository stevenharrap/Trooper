using Trooper.Thorny.Business.Operation.Core;
using Trooper.Testing.ShopModel.Poco;
using Trooper.Testing.ShopModel.Model;
using Trooper.Testing.CustomShopApi.Interface.Business.Support.InventorySupport;

namespace Trooper.Testing.CustomShopApi.Business.Support.InventorySupport
{
    public class InventoryBusinessCore : BusinessCore<InventoryEnt, Inventory>, IInventoryBusinessCore
    {
    }
}
