using System;
namespace Trooper.Testing.ShopModel.Interface
{
    interface IInventory
    {
        string Bin { get; set; }

        int ProductId { get; set; }

        int Quantity { get; set; }

        int ShopId { get; set; }
    }
}
