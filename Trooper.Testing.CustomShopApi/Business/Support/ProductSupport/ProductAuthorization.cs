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
    using ShopModel.Poco;

    public class ProductAuthorization : Authorization<Product>, IProductAuthorization
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
