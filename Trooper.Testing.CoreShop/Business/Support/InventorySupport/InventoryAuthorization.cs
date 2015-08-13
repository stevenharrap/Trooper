namespace Trooper.Testing.CustomShopApi.Business.Support.InventorySupport
{
    using System.Collections.Generic;
    using Thorny.Business.Security;
    using Trooper.Interface.Thorny.Business.Security;
    using ShopModel.Model;
    using Trooper.Testing.CustomShopApi.Business.Support.OutletSupport;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.InventorySupport;

    public class InventoryAuthorization : Authorization<InventoryEnt>, IInventoryAuthorization
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
