namespace Trooper.Testing.CustomShopApi.Business.Support.InventorySupport
{
    using System.Collections.Generic;
    using Thorny.Business.Security;
    using Trooper.Interface.Thorny.Business.Security;
    using ShopModel.Model;
    using Trooper.Testing.CustomShopApi.Business.Support.OutletSupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.InventorySupport;
    using ShopModel.Poco;

    public class InventoryAuthorization : Authorization<Inventory>, IInventoryAuthorization
    {
	    public override IList<IAssignment> Assignments
	    {
			get
			{
                return OutletAuthorization.GeneralAssigments;
			}
	    }
    }
}
