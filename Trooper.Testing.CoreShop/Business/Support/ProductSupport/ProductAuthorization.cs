namespace Trooper.Testing.CustomShopApi.Business.Support.ProductSupport
{
    using System.Collections.Generic;
    using Thorny.Business.Security;
    using Thorny.UnitTestBase;
    using Trooper.Interface.Thorny.Business.Security;
    using Interface.Business.Support;
    using ShopModel.Model;
    using Trooper.Testing.CustomShopApi.Interface.Business.Support.ProductSupport;
    using Trooper.Testing.CustomShopApi.Business.Support.OutletSupport;

    public class ProductAuthorization : Authorization<ProductEnt>, IProductAuthorization
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
