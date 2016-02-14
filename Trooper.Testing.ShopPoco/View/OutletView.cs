namespace Trooper.Testing.ShopPoco.View
{
    using System.Collections.Generic;

    public class OutletView
    {
        public Outlet Outlet { get; set; }

        public IEnumerable<InventoryView> Inventory { get; set; }
    }    
}