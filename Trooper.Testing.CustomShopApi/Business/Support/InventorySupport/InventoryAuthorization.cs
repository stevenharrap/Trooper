namespace Trooper.Testing.CustomShop.Api.Business.Support.InventorySupport
{
    using System.Collections.Generic;
    using Thorny.Business.Security;
    using Trooper.Interface.Thorny.Business.Security;
    using ShopPoco;
    using Interface.Business.Support.InventorySupport;
    using OutletSupport;

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
